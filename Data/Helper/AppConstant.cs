using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Helper
{
    public class AppConstant
    {
        public struct DelFlg
        {
            public const string HIDE = "1";
            public const string UNHIDE = "0";


        }
        public struct Status
        {
            public const string ALL = "0";
        }

        
        public struct ENVIRONMENT_VARIABLE
        {
            public const int SUCCESS = 1;
            public const int FAIL = 0;
            public const int NOT_FOUND = -1;
            public const int DUPLICATE = 2;
            public const string DELETED = "1";
            public const string UNLIMIT = "0";
            public const string ISLIMIT = "1";            
            public struct CHARSET_TYPE
            {
                public const string ALPHANUMERIC = "Alphanumeric";
                public const string ALPHABETIC = "Alphabetic";
                public const string ALPHABETIC_LOWERCASE = "Alphabetic Lowercase";
                public const string ALPHABETIC_UPERCASE = "Alphabetic Uppercase";
                public const string NUMBERS = "Numbers";
                public const string CUSTOM = "Custom";
            }
            public static class CHARSET_CHARS
            {
                public const string ALPHANUMERIC = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
                public const string ALPHABETIC = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
                public const string ALPHABETIC_LOWERCASE = "abcdefghijklmnopqrstuvwxyz";
                public const string ALPHABETIC_UPERCASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                public const string NUMBERS = "0123456789";
            }
            public struct VOUCHER_TYPE
            {
                public const string BULK_CODE = "1";
                public const string STANDALONE_CODE = "2";
            }
            public struct PROMOTION_TYPE
            {
                public const string DISCOUNT = "1";
                public const string PROMOTION = "2";
                public const string GIFT = "3";
                public const string BONUS_POINT = "4";
            }
            public struct PROMOTION_STATUS
            {
                public const string DRAFT = "1";
                public const string PUBLISH = "2";
                public const string EXPIRED = "3";
            }
            public struct EXCLULSIVE
            {
                public const string NONE = "1";
                public const string LEVEL = "2";
                public const string GLOBAL = "3";
            }
        }

    }
}
