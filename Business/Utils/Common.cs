using Infrastructure.DTOs;
using Infrastructure.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Utils
{
    public static class Common
    {
        /*  public static Boolean Operator(this string logic, decimal a, decimal b)
          {
              return logic switch
              {
                  AppConstant.Operator.GREATER_THAN => a > b,
                  AppConstant.Operator.GREATER_THAN_OR_EQUAL => a >= b,
                  AppConstant.Operator.LESS_THAN => a < b,
                  AppConstant.Operator.LESS_THAN_OR_EQUAL => a <= b,
                  AppConstant.Operator.EQUAL => a == b,
                  _ => throw new ErrorObj(code: 400, message: AppConstant.ErrMessage.Invalid_Operator),
              };

          }*/
        public static bool Compare<T>(string op, T orderAmount, T minAmount) where T : IComparable<T>
        {
            return op switch
            {
                AppConstant.Operator.GREATER_THAN => orderAmount.CompareTo(minAmount) > 0,
                AppConstant.Operator.GREATER_THAN_OR_EQUAL => orderAmount.CompareTo(minAmount) >= 0,
                AppConstant.Operator.LESS_THAN => orderAmount.CompareTo(minAmount) < 0,
                AppConstant.Operator.LESS_THAN_OR_EQUAL => orderAmount.CompareTo(minAmount) <= 0,
                AppConstant.Operator.EQUAL => orderAmount.CompareTo(minAmount) == 0,
                _ => throw new ErrorObj(code: 400, message: AppConstant.ErrMessage.Invalid_Operator),
            };
        }
        public static DateTime GetCurrentDatetime()
        {
            return DateTime.UtcNow.AddHours(7);
        }

        public static bool CompareBinary(string input, string compareString)
        {
            int input_Decimal = Int32.Parse(input);
            int compareString_Decimal = Int32.Parse(compareString);
            return (input_Decimal & compareString_Decimal) == input_Decimal;
        }      
        
    }
}
