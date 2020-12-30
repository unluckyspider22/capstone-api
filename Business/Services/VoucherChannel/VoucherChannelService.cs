using ApplicationCore.Models.VoucherChannel;
using ApplicationCore.Utils;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class VoucherChannelService : IVoucherChannelService
    {
        private readonly PromotionEngineContext _context;
        public VoucherChannelService(PromotionEngineContext context)
        {
            _context = context;
        }

        public int CountVoucherChannel()
        {
            return _context.VoucherChannel.ToList().Count;
        }

        public int DeleteVoucherChannel(Guid id)
        {
            var VoucherChannel = _context.VoucherChannel.Find(id);

            if (VoucherChannel == null)
            {
                return GlobalVariables.NOT_FOUND;
            }
            try
            {
                _context.VoucherChannel.Remove(VoucherChannel);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }


            return GlobalVariables.SUCCESS;
        }

        public VoucherChannel GetVoucherChannel(Guid id)
        {
            var VoucherChannel = _context.VoucherChannel.Find(id);
            /* VoucherChannelParam result = new VoucherChannelParam();
             result.VoucherChannelId = id;
             result.VoucherChannelCode = VoucherChannel.VoucherChannelCode;
             result.VoucherChannelName = VoucherChannel.VoucherChannelName;
             result.BrandId = VoucherChannel.BrandId;*/
            if (VoucherChannel.DelFlg.Equals(GlobalVariables.DELETED))
            {
                // vẫn trả ra obj, FE check delflag để hiển thị ????
                return null;
            }
            return VoucherChannel;
        }

        public List<VoucherChannel> GetVoucherChannels()
        {
            return _context.VoucherChannel.Where(c => !c.DelFlg.Equals(GlobalVariables.DELETED)).ToList();
        }

        public int PostVoucherChannel(VoucherChannel VoucherChannel)
        {
            VoucherChannel.VoucherChannelId = Guid.NewGuid();
            VoucherChannel.InsDate = DateTime.Now;
            _context.VoucherChannel.Add(VoucherChannel);
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (VoucherChannelExists(VoucherChannel.VoucherChannelId))
                {
                    return GlobalVariables.DUPLICATE;
                }
                else
                {
                    throw;
                }
            }

            return GlobalVariables.SUCCESS;
        }

        public int PutVoucherChannel(Guid id, VoucherChannelParam VoucherChannelParam)
        {
            var result = _context.VoucherChannel.Find(id);

            if (result == null)
            {
                return GlobalVariables.NOT_FOUND;
            }
            result.VoucherChannelId = VoucherChannelParam.VoucherChannelId;
            result.VoucherGroupId = VoucherChannelParam.VoucherGroupId;
            result.PromotionId = VoucherChannelParam.PromotionId;
            result.UpdDate = DateTime.Now;
            result.ChannelId = VoucherChannelParam.ChannelId;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VoucherChannelExists(id))
                {
                    return GlobalVariables.NOT_FOUND;
                }
                else
                {
                    throw;
                }
            }

            return GlobalVariables.SUCCESS;
        }
        public int UpdateDelFlag(Guid id, string delflg)
        {
            var VoucherChannel = _context.VoucherChannel.Find(id);
            if (VoucherChannel == null)
            {
                return GlobalVariables.NOT_FOUND;
            }
            VoucherChannel.UpdDate = DateTime.Now;
            VoucherChannel.DelFlg = delflg;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {

                throw;

            }
            return GlobalVariables.SUCCESS;
        }

        private bool VoucherChannelExists(Guid id)
        {
            return _context.VoucherChannel.Any(e => e.VoucherChannelId == id);
        }
    }
}
