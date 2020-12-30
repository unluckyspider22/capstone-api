using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Utils;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace ApplicationCore.Services
{
    public class HolidayService : IHolidayService
    {
        private readonly PromotionEngineContext _context;
        public HolidayService(PromotionEngineContext context)
        {
            _context = context;
        }
        public int CountHoliday()
        {
            return _context.Holiday.Where(c => !c.DelFlg.Equals(GlobalVariables.DELETED)).Count();
        }

        public int CreateHoliday(Holiday holiday)
        {

            holiday.HolidayId = Guid.NewGuid();

            _context.Holiday.Add(holiday);
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return GlobalVariables.SUCCESS;
        }

        public int DeleteHoliday(Guid id)
        {
            var holiday = _context.Holiday.Find(id);

            if (holiday == null)
            {
                return GlobalVariables.NOT_FOUND;
            }

            
            try
            {
                _context.Holiday.Remove(holiday);
            }
            catch (Exception)
            {
                throw;
            }


            return GlobalVariables.SUCCESS;
        }

        public List<Holiday> GetHoliday()
        {
            return _context.Holiday.Where(c => !c.DelFlg.Equals(GlobalVariables.DELETED)).ToList();
        }

        public Holiday GetHolidays(Guid id)
        {
            return _context.Holiday
              .Where(c => !c.DelFlg.Equals(GlobalVariables.DELETED))
              .Where(c => c.HolidayId.Equals(id))
              .First();
        }

        public int HideHoliday(Guid id)
        {
            var holiday = _context.Holiday.Find(id);

            if (holiday == null)
            {
                return GlobalVariables.NOT_FOUND;
            }

            holiday.DelFlg = GlobalVariables.DELETED;
            holiday.UpdDate = DateTime.Now;
            try
            {
                _context.Entry(holiday).Property("DelFlg").IsModified = true;
                _context.Entry(holiday).Property("UpdDate").IsModified = true;
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }


            return GlobalVariables.SUCCESS;
        }

        public int UpdateHoliday(Guid id, Holiday holiday)
        {
            var h = _context.Holiday.Find(id);

            if (h == null)
            {
                return GlobalVariables.NOT_FOUND;
            }

            h.HolidayName = holiday.HolidayName;
            h.Rank = holiday.Rank;
            h.UpdDate = DateTime.Now;

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
    }
}
