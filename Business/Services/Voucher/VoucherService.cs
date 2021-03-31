
using ApplicationCore.Request;
using ApplicationCore.Utils;
using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.DTOs.Voucher;
using Infrastructure.DTOs.VoucherChannel;
using Infrastructure.Helper;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class VoucherService : BaseService<Voucher, VoucherDto>, IVoucherService
    {
        IVoucherGroupService _voucherGroupService;
        IMembershipService _membershipService;

        public VoucherService(IUnitOfWork unitOfWork, IMapper mapper, IVoucherGroupService voucherGroupService, IMembershipService membershipService) : base(unitOfWork, mapper)
        {
            _voucherGroupService = voucherGroupService;
            _membershipService = membershipService;
        }
        protected override IGenericRepository<Voucher> _repository => _unitOfWork.VoucherRepository;
        protected IGenericRepository<VoucherGroup> _voucherGroupRepos => _unitOfWork.VoucherGroupRepository;


        public async Task<List<Promotion>> CheckVoucher(CustomerOrderInfo order)
        {
            try
            {
                var vouchers = order.Vouchers;
                if (vouchers.Select(el => new { el.VoucherCode, el.PromotionCode }).Distinct().Count() < vouchers.Select(el => new { el.VoucherCode, el.PromotionCode }).Count())
                {
                    throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Duplicate_VoucherCode, description: AppConstant.ErrMessage.Duplicate_VoucherCode);
                }
                var promotions = new List<Promotion>();
                foreach (var voucherModel in vouchers)
                {
                    var voucher = await _repository.Get(filter: el =>
                    el.VoucherCode.Equals(voucherModel.VoucherCode)
                    && el.Promotion.Brand.BrandCode.Equals(order.Attributes.StoreInfo.BrandCode)
                    && el.Promotion.PromotionCode.Equals(voucherModel.PromotionCode)
                    && !el.IsUsed,
                    includeProperties:
                    "Promotion.PromotionTier.Action.ActionProductMapping.Product," +
                    "Promotion.PromotionTier.PostAction.PostActionProductMapping.Product," +
                    "Promotion.PromotionTier.Action.ActionProductMapping.Product," +
                    "Promotion.PromotionTier.ConditionRule.ConditionGroup.OrderCondition," +
                    "Promotion.PromotionTier.ConditionRule.ConditionGroup.ProductCondition.ProductConditionMapping.Product," +
                    "Promotion.PromotionStoreMapping.Store," +
                    "Promotion.Brand");
                    if (voucher.Count() > 1)
                    {
                        throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Duplicate_VoucherCode, description: AppConstant.ErrMessage.Duplicate_VoucherCode);
                    }
                    if (voucher.Count() == 0)
                    {
                        throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Invalid_VoucherCode, description: AppConstant.ErrMessage.Invalid_VoucherCode);
                    }
                    var promotion = voucher.First().Promotion;
                    promotions.Add(promotion);
                }
                if (promotions.Select(s => s.PromotionId).Distinct().Count() < promotions.Select(s => s.PromotionId).Count())
                {
                    throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Duplicate_Promotion);
                }
                return promotions;
            }
            catch (ErrorObj e1)
            {
                throw e1;
            }
            catch (Exception e)
            {

                throw new ErrorObj(code: 500, message: e.Message, description: "Internal Server Error");
            }


        }
        #region Lấy voucher cho channel
        public async Task<List<Voucher>> GetVouchersForChannel(PromotionChannelMapping voucherChannel, VoucherGroup voucherGroup, VoucherChannelParam channelParam)
        {

            int remainVoucher = (int)(voucherGroup.Quantity - voucherGroup.RedempedQuantity);
            if (remainVoucher == 0)
            {
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Voucher_OutOfStock);
            }
            if (channelParam.Quantity > remainVoucher)
            {
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Invalid_VoucherQuantity + remainVoucher);
            }

            var vouchers = await _repository.Get(
                pageSize: channelParam.Quantity,
                pageIndex: 1,
                filter: el =>
                   !el.IsRedemped
                   && !el.IsUsed
                   && el.VoucherGroupId.Equals(voucherGroup.VoucherGroupId));
            if (vouchers.Count() > 0)
            {
                foreach (var voucher in vouchers)
                {
                    voucher.IsRedemped = true;
                    var now = DateTime.Now;
                    voucher.RedempedDate = now;
                    voucher.UpdDate = now;
                    voucher.ChannelId = voucherChannel.ChannelId;
                    _repository.Update(voucher);
                }
                await _unitOfWork.SaveAsync();
            }
            return vouchers.ToList();
        }
        #endregion
        #region Lấy voucher cho customer
        public async Task<VoucherParamResponse> GetVoucherForCustomer(VoucherGroupDto voucherGroupDto)
        {
            try
            {
                if (voucherGroupDto != null)
                {
                    //var entity = _mapper.Map<VoucherGroup>(voucherGroupDto);
                    var vouchers = await _repository.Get(filter: el => el.VoucherGroupId.Equals(voucherGroupDto.VoucherGroupId)
                    && el.IsRedemped == AppConstant.EnvVar.Voucher.UNREDEEM, includeProperties: "VoucherGroup.Promotion");
                    var voucher = vouchers.ToList().First();
                    voucher.IsRedemped = AppConstant.EnvVar.Voucher.REDEEMPED;
                    voucher.RedempedDate = DateTime.Now;
                    _repository.Update(voucher);
                    await _unitOfWork.SaveAsync();
                    //await UpdateVoucherGroupAfterRedeemed(entity);
                    var entity = voucher.VoucherGroup;
                    entity.RedempedQuantity += 1;
                    entity.UpdDate = DateTime.Now;
                    _voucherGroupRepos.Update(entity);
                    await _unitOfWork.SaveAsync();
                    var promoCode = voucher.Promotion.PromotionCode;
                    var description = voucher.Promotion.Description;
                    return new VoucherParamResponse(voucherGroupId: entity.VoucherGroupId, voucherGroupName: entity.VoucherName,
                        voucherId: voucher.VoucherId, code: promoCode + "-" + voucher.VoucherCode, description: description);
                }
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: null);
            }
            catch (Exception e)
            {

                throw new ErrorObj(code: (int)HttpStatusCode.InternalServerError, message: e.Message);
            }
        }

        #endregion
        #region Update voucher đã applied
        public async Task<List<Voucher>> UpdateVoucherApplied(CustomerOrderInfo order)
        {

            try
            {
                List<Voucher> result = null;
                if (order != null)
                {
                    var promotions = await CheckVoucher(order);
                    result = new List<Voucher>();
                    foreach (var voucherParam in order.Vouchers)
                    {
                        foreach (var promotion in promotions)
                        {
                            if (voucherParam.PromotionCode.Equals(promotion.PromotionCode))
                            {
                                var voucherGroup = (await _voucherGroupService.
                                    GetAsync(/*filter: el => el.PromotionId.Equals(promotion.PromotionId), */includeProperties: "Voucher")).Data.First();
                                if (voucherGroup.Voucher.Where(el => el.VoucherCode.Equals(voucherParam.VoucherCode)).Distinct().Count()
                                < voucherGroup.Voucher.Where(w => w.VoucherCode.Equals(voucherParam.VoucherCode)).Count())
                                {
                                    throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Duplicate_VoucherCode,
                                        description: AppConstant.ErrMessage.Duplicate_VoucherCode);
                                }
                                var vouchers = voucherGroup.Voucher.Where(w => w.VoucherCode.Equals(voucherParam.VoucherCode)).First();
                                if (voucherGroup.VoucherType.Equals(AppConstant.EnvVar.VoucherType.STANDALONE_CODE))
                                {
                                    await UpdateVoucherGroupAfterApplied(voucherGroup);
                                }
                                else
                                {
                                    await UpdateVoucherGroupAfterApplied(voucherGroup);
                                    await UpdateVoucherAfterApplied(vouchers, order);
                                }
                                result.Add(vouchers);
                                return result;
                            }
                        }
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                throw new ErrorObj(code: (int)HttpStatusCode.InternalServerError, message: e.Message);
            }

        }
        private async Task UpdateVoucherGroupAfterApplied(VoucherGroup voucherGroup)
        {
            voucherGroup.UpdDate = DateTime.Now;
            voucherGroup.UsedQuantity += 1;
            await _voucherGroupService.UpdateVoucherGroupForApplied(voucherGroup);
        }
        private async Task UpdateVoucherAfterApplied(Voucher voucher, CustomerOrderInfo order)
        {
            voucher.UpdDate = DateTime.Now;
            voucher.UsedDate = DateTime.Now;
            voucher.IsUsed = AppConstant.EnvVar.Voucher.USED;
            if (order.Customer != null)
            {
                var memInfor = order.Customer;
                MembershipDto membership = new MembershipDto();
                membership.MembershipId = Guid.NewGuid();
                membership.InsDate = DateTime.Now;
                membership.Fullname = memInfor.CustomerName;
                membership.Email = memInfor.CustomerEmail;
                membership.PhoneNumber = memInfor.CustomerPhoneNo;
                var result = await _membershipService.CreateAsync(membership);
                voucher.MembershipId = result.MembershipId;
            }
            _repository.Update(voucher);
            await _unitOfWork.SaveAsync();
        }
        #endregion
        #region Lấy voucher cho customer qua Chatbot
        public async Task<VoucherForCustomerModel> GetVoucherForCusOnSite(VoucherForCustomerModel param, Guid promotionId, string storeCode)
        {
            try
            {
                var vouchers = await _repository.Get(
                    filter: el =>
                    el.PromotionId == promotionId
                    && !el.IsUsed
                    && !el.IsRedemped
                    && !el.Promotion.DelFlg
                    && el.Promotion.Status == (int)AppConstant.EnvVar.PromotionStatus.PUBLISH,
                    includeProperties:
                    "VoucherGroup.Promotion.PromotionChannelMapping.Channel," +
                    "VoucherGroup.Promotion.PromotionStoreMapping.Store," +
                    "VoucherGroup.Brand.UsernameNavigation," +
                    "Membership");
                var voucher = new Voucher();
                if (vouchers.Count() > 0)
                {
                    voucher = vouchers.FirstOrDefault();
                    await SendEmailSmtp(param, voucher);

                    //Update voucher vừa lấy
                    await UpdateVoucherRedemped(voucher, param, storeCode);
                }
                else
                {
                    throw new ErrorObj(code: (int)HttpStatusCode.Gone, message: AppConstant.ErrMessage.Voucher_OutOfStock);
                }
                return param;
            }
            catch (ErrorObj e1)
            {
                throw e1;
            }
            catch (Exception e)
            {
                throw new ErrorObj(code: (int)HttpStatusCode.InternalServerError, message: e.Message);
            }


        }


        private async Task SendEmailSmtp(VoucherForCustomerModel param, Voucher voucher)
        {

            //Tạo người gửi - nhận
            MimeMessage message = new MimeMessage();
            MailboxAddress from = new MailboxAddress(AppConstant.Sender, AppConstant.Sender_Email);
            message.From.Add(from);

            MailboxAddress to = new MailboxAddress(param.CusFullName, param.CusEmail);
            message.To.Add(to);

            message.Subject = AppConstant.Subject;

            //Tạo nội dung email
            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = GenerateContent(param, voucher);
            message.Body = bodyBuilder.ToMessageBody();

            //Kết nối tới SMTP server
            SmtpClient client = new SmtpClient();
            try
            {
                client.Connect("smtp.gmail.com", 465, true);
                client.Authenticate(AppConstant.Sender_Email, AppConstant.Sender_Email_Pwd);
                await client.SendAsync(message);

            }
            catch (Exception e)
            {
                throw new ErrorObj(code: (int)HttpStatusCode.InternalServerError, e.Message);
            }
            finally
            {
                //Teminate
                client.Disconnect(true);
                client.Dispose();
            }


        }

        private string GenerateContent(VoucherForCustomerModel param, Voucher voucher)
        {
            var promotion = voucher.Promotion;
            var brand = voucher.VoucherGroup.Brand;
            //Header
            string header = string.Format("<h1>[Promotion] {0}</h1>", promotion.PromotionName);
            //Opening
            string dearGender = param.CusGender == "Male" ? "Mr." : "Ms.";
            string openning = string.Format(@"<p>Dear <b>{0} {1},</b></p>", dearGender, param.CusFullName);
            //Preface
            string preface = string.Format("<p>Thank you for your submission. " +
                "Hope that you enjoy this promotion of <b>{0}</b>. Detail of your voucher is below:<p>", brand.BrandName);
            //Body
            string voucherCode = promotion.PromotionCode + "-" + voucher.VoucherCode;
            string QrCode = AppConstant.Url_Gen_QR + voucherCode;
            string body = string.Format("<p>Promotion Code: <b>{0}</b></p>" +
                "<p>Voucher Code: <b>{1}</b></p>" +
                "<p>Description:</p>" +
                "<p>{2}</p>" +
                "<p>Or you can use the code below:</p>" +
                "<img src={3}><br>", promotion.PromotionCode, voucherCode, promotion.Description, QrCode);
            //Note
            string note = string.Format("<p>Note:</p>" +
                "<ul>" +
                "<li>This promotion may end before the duration</li>" +
                "<li>If have any questions, please call the hotline <b>{0}</b> or email <a href=\"mailto: {1}\" target=\"_blank\">{1}</a></li>" +
                "</ul>", brand.PhoneNumber, brand.UsernameNavigation.Email);

            //Footer
            string footer = "<p>Regards,</p>" +
                "<p>PMSR Team</p>";
            string emailContent = header + openning + preface + body + note + footer;

            return emailContent;
        }
        public async Task UpdateVoucherRedemped(Voucher voucher, VoucherForCustomerModel param, string storeCode)
        {
            if (!string.IsNullOrEmpty(storeCode))
            {
                //Update store
                var store = voucher.Promotion.PromotionStoreMapping.FirstOrDefault(w => w.Store.StoreCode == storeCode).Store;
                voucher.Store = store;
            }
            else
            {
                //Update channel
                var channel = voucher.Promotion.PromotionChannelMapping.FirstOrDefault(w => w.Channel.ChannelCode == param.ChannelCode).Channel;
                voucher.Channel = channel;
            }

            //Update membership
            MembershipDto membership = new MembershipDto
            {
                MembershipId = Guid.NewGuid(),
                Email = param.CusEmail,
                Fullname = param.CusFullName,
                PhoneNumber = param.CusPhoneNo
            };
            var result = await _membershipService.CreateAsync(membership);
            voucher.MembershipId = membership.MembershipId;

            // Update ngày lấy
            DateTime now = Common.GetCurrentDatetime();
            voucher.RedempedDate = now;
            voucher.IsRedemped = true;
            voucher.UpdDate = now;
            //Update voucher group
            voucher.VoucherGroup.RedempedQuantity += 1;
            voucher.VoucherGroup.UpdDate = now;

            _repository.Update(voucher);
            await _unitOfWork.SaveAsync();
        }


        #endregion

        public string Encrypt(string Encryptval)
        {
            return Common.EncodeToBase64(Encryptval);
        }
        public string Decrypt(string DecryptText)
        {
            return Common.DecodeFromBase64(DecryptText);
        }
    }
}

