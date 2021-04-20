using ApplicationCore.Services;
using ApplicationCore.Utils;
using Infrastructure.DTOs;
using Infrastructure.DTOs.VoucherChannel;
using Infrastructure.Helper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/channels")]
    [ApiController]
    public class ChannelsController : ControllerBase
    {
        private readonly IChannelService _service;

        public ChannelsController(IChannelService service)
        {
            _service = service;
        }

        // GET: api/Channels
        [HttpGet]
        public async Task<IActionResult> GetChannel([FromQuery] PagingRequestParam param, [FromQuery] Guid BrandId)
        {
            var result = await _service.GetAsync(
                pageIndex: param.PageIndex,
                pageSize: param.PageSize,
                filter: el => !el.DelFlg && el.BrandId.Equals(BrandId),
                orderBy: el => el.OrderByDescending(b => b.InsDate));

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpPost]
        [Route("checkChannelCodeExist")]
        public async Task<IActionResult> CheckChannelExisting([FromBody] DuplicateParam param)
        {
            bool isExisting = false;
            isExisting = (await _service.GetAsync(filter: el =>
                    el.BrandId == param.BrandID
                   && (param.ChannelId != Guid.Empty ? (el.ChannelId != param.ChannelId && el.ChannelCode == param.ChannelCode) : (el.ChannelCode == param.ChannelCode) && !el.DelFlg)
                   && !el.DelFlg)).Data.Count > 0;
            return Ok(isExisting);
        }
        // GET: api/Channels/count
        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> CountChannel([FromQuery] Guid BrandId)
        {
            return Ok(await _service.CountAsync(el => !el.DelFlg && el.BrandId.Equals(BrandId)));
        }

        // GET: api/Channels/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetChannel([FromRoute]Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // PUT: api/Channels/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChannel([FromRoute]Guid id, [FromBody] ChannelDto dto)
        {
            if (id != dto.ChannelId)
            {
                return BadRequest();
            }

            dto.UpdDate = DateTime.Now;

            var result = await _service.UpdateAsync(dto);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);

        }

        // POST: api/Channels
        [HttpPost]
        public async Task<IActionResult> PostChannel([FromBody] ChannelDto dto)
        {
            dto.ChannelId = Guid.NewGuid();
            if (dto.ChannelType != (int)AppConstant.ChannelType.Other)
            {
                dto.ApiKey = Common.CreateApiKey();
            }
            else
            {
                RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(2048);
                dto.PublicKey = RSACryptoUtils.ExportPublicKey(rsaProvider);
                dto.PrivateKey = RSACryptoUtils.ExportPrivateKey(rsaProvider);
            }
            var result = await _service.CreateAsync(dto);
            if (result == null)
            {
                return NotFound();
            }

            //var result = dto;

            return Ok(result);
        }

        // DELETE: api/Channels/5
        [HttpDelete]
        public async Task<IActionResult> DeleteChannel([FromQuery]Guid id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var result = await _service.DeleteAsync(id);
            if (result == false)
            {
                return NotFound();
            }
            return Ok();
        }
        [HttpGet]

        [Route("{channelCode}/vouchers/{promotionId}")]
        public async Task<IActionResult> GetVoucherForChannel(Guid promotionId, string channelCode, [FromBody] VoucherChannelParam param)
        {
            try
            {
                if (channelCode != param.ChannelCode)
                {
                    return NotFound();
                }
                if (promotionId != param.PromotionId)
                {
                    return NotFound();
                }
                var result = await _service.GetVouchersForChannel(param);
                return Ok(result);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }

        [HttpGet]
        [Route("{channelCode}/brands/{BrandCode}/promotions")]
        public async Task<IActionResult> GetPromotionForChannel(string channelCode, string BrandCode, [FromQuery] string key)
        {
            try
            {
                VoucherChannelParam param = new VoucherChannelParam
                {
                    ChannelCode = channelCode,
                    BrandCode = BrandCode
                };
                var channel = await _service.GetFirst(filter: el => el.ChannelCode == channelCode);
                if (channel != null)
                {
                    if (string.IsNullOrEmpty(key))
                    {
                        return Unauthorized(new ErrorObj(code: (int)HttpStatusCode.NotFound, message: AppConstant.ErrMessage.ApiKey_Required));
                    }
                    if (channel.ApiKey.Equals(key))
                    {
                        return Ok(await _service.GetPromotionsForChannel(param));
                    }
                    else
                    {
                        return Unauthorized(new ErrorObj(code: (int)HttpStatusCode.Unauthorized, message: AppConstant.ErrMessage.ApiKey_Not_Exist));
                    }
                }
                return StatusCode(statusCode: (int)HttpStatusCode.NotFound,
                    new ErrorObj(code: (int)HttpStatusCode.NotFound, message: AppConstant.ErrMessage.Not_Found_Resource));

            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }

        }

        [HttpGet]
        [Route("promotion/{promotionId}")]
        public async Task<IActionResult> GetChannelOfPromotion([FromRoute] Guid promotionId, [FromQuery] Guid brandId)
        {
            if (promotionId.Equals(Guid.Empty) || brandId.Equals(Guid.Empty))
            {
                return StatusCode(statusCode: (int)HttpStatusCode.BadRequest, new ErrorResponse().BadRequest);
            }
            try
            {

                return Ok(await _service.GetChannelOfPromotion(promotionId: promotionId, brandId: brandId));
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }

        [HttpPut]
        [Route("promotion/{promotionId}")]
        public async Task<IActionResult> UpdateChannelOfPromotion([FromRoute] Guid promotionId, [FromBody] UpdateChannelOfPromotion dto)
        {
            if (promotionId.Equals(Guid.Empty) || !promotionId.Equals(dto.PromotionId))
            {
                return StatusCode(statusCode: (int)HttpStatusCode.BadRequest, new ErrorResponse().BadRequest);
            }
            try
            {
                return Ok(await _service.UpdateChannelOfPromotion(dto: dto));
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }
        [HttpPost]
        [Route("encrypt")]
        public async Task<IActionResult> EncryptData([FromBody] string json)
        {
            RSACryptoServiceProvider publicK = RSACryptoUtils.ImportPublicKey("ac");
            string publicKey = RSACryptoUtils.ExportPublicKey(publicK);
            byte[] bytesPlainTextData = Encoding.UTF8.GetBytes(json);
            var bytesCipherText = publicK.Encrypt(bytesPlainTextData, false);
            string encryptedText = Convert.ToBase64String(bytesCipherText);
            return Ok(encryptedText);
        }

        [HttpPost]
        [Route("decrypt")]
        public async Task<IActionResult> DecryptData([FromBody] string encryptText)
        {
            /*var plainText = RSACryptoUtils.Decryption(encryptText);*/
            RSACryptoServiceProvider privateK = RSACryptoUtils.ImportPrivateKey("ac");
            byte[] bytesCipherText = Convert.FromBase64String(encryptText);
            byte[] bytesPlainTextData = privateK.Decrypt(bytesCipherText, false);
            //get our original plainText back...
            return Ok(Encoding.UTF8.GetString(bytesPlainTextData));
        }
    }
}