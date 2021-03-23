using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Helper
{
    public static class AppConstant
    {
        public struct DelFlg
        {
            public const string HIDE = "1";
            public const string UNHIDE = "0";


        }
        public const string ACTIVE = "1";
        public const string UNREDEMPED = "0";
        public const string UNUSED = "0";
        public const string USED = "1";
        public struct Status
        {
            public const string ALL = "0";
        }

        public const string Sender = "Promotion Engine";
        public const string Sender_Email = "promotion.engine.fpt@gmail.com";
        public const string Sender_Email_Pwd = "promotionengine";
        public const string Subject = "Promotion Engine send you a voucher. Enjoy it!";
        public const string Url_Gen_QR = "http://chart.googleapis.com/chart?cht=qr&chs=300x300&chl=";

        public struct EnvVar
        {
            public const int SUCCESS = 1;
            public const int FAIL = 0;
            public const int NOT_FOUND = -1;
            public const int DUPLICATE = 2;
            public const string DELETED = "1";
            public const string UNLIMIT = "0";
            public const string CONNECTOR = "-";
            public const bool ISLIMIT = true;
            public const bool IS_FOR_GAME = true;
            public const string FOR_WEEKEND = "1";
            public const string FOR_HOLIDAY = "1";
            public const string NO_FILTER = "0";
            public const string FOR_MEMBER = "1";
            public const string INCLUDE = "0";
            public const string EXCLUDE = "1";
            public const string ApplyForAllStore = "Khuyến mãi áp dụng tại tất cả cửa hàng toàn quốc";
            public const int BrandId = 1;
            public const int AdminId = 2;
            public const string Admin = "Admin";
            public const string BrandManager = "Brand Manager";
            public struct ActionType
            {
                public const string Product = "1";
                public const string Order = "2";
                public const string Gift = "3";
                public const string BonusPoint = "4";
            }

            public struct DiscountType
            {
                public const string Amount = "1";
                public const string Percentage = "2";
                public const string Unit = "3";
                public const string Shipping = "4";
                public const string Fixed = "5";
                public const string Ladder = "6";
                public const string Bundle = "7";
                public const string GiftProduct = "8";
                public const string GiftVoucher = "9";
                public const string BonusPoint = "10";
            }
            public struct CharsetType
            {
                public const string ALPHANUMERIC = "Alphanumeric";
                public const string ALPHABETIC = "Alphabetic";
                public const string ALPHABETIC_LOWERCASE = "Alphabetic Lowercase";
                public const string ALPHABETIC_UPERCASE = "Alphabetic Uppercase";
                public const string NUMBERS = "Numbers";
                public const string CUSTOM = "Custom";
            }
            public struct CharsetChars
            {
                public const string ALPHANUMERIC = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
                public const string ALPHABETIC = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
                public const string ALPHABETIC_LOWERCASE = "abcdefghijklmnopqrstuvwxyz";
                public const string ALPHABETIC_UPERCASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                public const string NUMBERS = "0123456789";
            }
            public struct ProductType
            {
                public const string SINGLE_PRODUCT = "1";
                public const string COLLECTION = "2";
                public const string CATEGORY = "3";
                public const string TAG = "4";
                public const string GENERAL = "5";
                public const string COMBO = "6";
                public const string COMPLEX = "7";
                public const string EXTRA = "8";
            }

            public struct VoucherType
            {
                public const string BULK_CODE = "1";
                public const string STANDALONE_CODE = "2";
            }
            public struct Voucher
            {
                public const bool USED = true;
                public const bool UNUSED = false;
                public const bool REDEEMPED = true;
                public const bool UNREDEEM = false;
            }
            public struct PromotionType
            {
                public const string DISCOUNT = "1";
                public const string AUTO_PROMOTION = "2";
                public const string GIFT = "3";
                public const string BONUS_POINT = "4";
            }
            public struct PromotionStatus
            {
                public const string DRAFT = "1";
                public const string PUBLISH = "2";
                public const string UNPUBLISH = "3";
                public const string EXPIRED = "4";
            }

            public struct Exclusive
            {
                public const string NoneExclusive = "0";
                public const string GlobalExclusive = "1";
                public const string ClassExclusiveOrder = "2";
                public const string ClassExclusiveProduct = "3";
                public const string ClassExclusiveShipping = "4";
            }
            public struct Holiday
            {
                public const int FRIDAY = 5;
                public const int SATURDAY = 6;
                public const int SUNDAY = 0;
            }
        }
        public class ErrMessage
        {
            public const string Invalid_Product = "Sản phẩm không tồn tại!";
            public const string Invalid_Quantity = "Số lượng không phải kiểu số!";
            public const string Invalid_Min_Quantity = "Số lượng sản phẩm ít hơn số lượng quy định";
            public const string Invalid_Max_Quantity = "Số lượng sản phẩm nhiều hơn số lượng quy định";
            public const string Invalid_Extra = "Sản phẩm extra cần đi kèm với sản phẩm chính";
            public const string Invalid_NotExtra = "Sản phẩm extra không thuộc sản phẩm chính";
            public const string Invalid_Parent = "Không tìm thấy sản phẩm chính";
            public const string Invalid_Require = "Đơn hàng chưa có sản phẩm bắt buộc";
            public const string Invalid_Group = "Sản phẩm chưa được chọn nhóm";
            public const string Duplicate_VoucherCode = "Trùng mã voucher.";
            public const string Invalid_VoucherCode = "Mã voucher không tồn tại.";
            public const string Expire_Promotion = "Khuyến mãi tạm dừng.";
            public const string Exclusive_Promotion = "Khuyến mãi không dùng chung với các khuyến mãi khác";
            public const string Invalid_Gender = "Giới tính không phù hợp với khuyến mãi";
            public const string InActive_Promotion = "Khuyến mãi chưa được kích hoạt";
            public const string Unmatch_Promotion = "Không có khuyến mãi phù hợp với voucher này";
            public const string Invalid_Holiday = "Khuyến mãi không áp dụng vào ngày lễ.";
            public const string Invalid_SaleMode = "Khuyến mãi không dành cho loại đơn hàng này.";
            public const string Invalid_MinAmount = "Giá đơn hàng thấp hơn quy định.";
            public const string Invalid_Time = "Thời gian khuyến mãi không phù hợp";
            public const string Invalid_Early = "Thời gian khuyễn mãi vẫn chưa bắt đầu";
            public const string Invalid_HourFrame = "Khung giờ khuyến mãi không phù hợp";
            public const string Invalid_DayInWeek = "Ngày trong tuần của khuyến mãi không phù hợp";
            public const string Invalid_PaymentType = "Loại hình thanh toán không phù hợp.";
            public const string NotExisted_Product = "Đơn hàng không có sản phẩm yêu cầu.";
            public const string Quantity_Product = "Sản phẩm yêu cầu thấp hơn quy định.";
            public const string NotMatchCondition = "Đơn hàng không thỏa mãn các điều kiện của khuyến mãi.";
            public const string Invalid_Store = "Đơn hàng không áp dụng tại cửa hàng này.";
            public const string Duplicate_Promotion = "Khuyến mãi không áp dụng cho các voucher thuộc cùng một chương trình khuyến mãi.";

            public const string Invalid_TimeFrame = "Khuyến mãi không áp dụng vào thời gian này.";
            public const string Invalid_Operator = "Invalid Logic Operator";

            public const string Invalid_CustomerLevel = "Khuyến mãi không áp dụng cho loại khách hàng này";
            public const string Invalid_Effect = "Khuyến mãi không ảnh hưởng";
            public const string Invalid_ProductCondition = "Điều kiện sản phẩm không hợp lệ, xin hãy kiểm tra lại điều kiện lúc tạo";
            public const string Invalid_ProductAction = "Hành động khuyến mãi không hợp lệ, xin hãy kiểm tra lại hành động lúc tạo";
            public const string Invalid_VoucherQuantity = "Khuyến mãi không có đủ số lượng Voucher yêu cầu, số lượng còn lại ";
            public const string Voucher_OutOfStock = "Voucher đã được phát hết";
            public const string Login_Success = "Success";
            public const string Login_Fail = "Unauthorized";

            public const string Device_Access_Fail = "Không tìm thấy thiết bị, vui lòng thử lại";
            public const string Device_Access_Server_Fail = "Hệ thống xảy ra lỗi, vui lòng thử lại";
        }

        public class EffectMessage
        {
            //Khi tier thỏa hết điều kiện
            public const string AcceptCoupon = "acceptCoupon";
            //Auto apply promotion
            public const string AutoPromotion = "autoPromotion";
            //Chỉ có 1 set discount => action có discount lớn nhất tùy vào Discount Type
            public const string SetDiscount = "setDiscount";
            //Chỉ có 1 set shipping fee => action có discount lớn nhất tùy vào Discount Type
            public const string SetShippingFee = "setShippingFee";
            //Chỉ có 1 lần add điểm => action có discount lớn nhất tùy vào Discount Type
            public const string AddPoint = "addPoint";
            
            public const string AddGift = "addGift";

        }
        public class Operator
        {
            public const string GREATER_THAN = "1";
            public const string GREATER_THAN_OR_EQUAL = "2";
            public const string LESS_THAN = "3";
            public const string LESS_THAN_OR_EQUAL = "4";
            public const string EQUAL = "5";
            public const string AND = "2";
            public const string OR = "1";

        }
        public class BundleStrategy
        {
            public const string CHEAPEST = "1";
            public const string MOST_EXPENSIVE = "2";
            public const string DEFAULT = "3";

        }

        //public const string URL = "https://localhost:44367/";
        public const string URL = "https://promoengine.azurewebsites.net/";

        public struct NotiMess
        {
            public const string PROCESSING_MESS = "is processing";
            public const string PROCESSED_MESS = "is finished";
            public const string ERROR_MESS = "is error";
            public const string VOUCHER_GENERATE_MESS = "Generating voucher";
            public const string VOUCHER_INSERT_MESS = "Inserting voucher";
            public const string VOUCHER_DELETE_MESS = "Deleting voucher";
            public struct Type
            {
                public const string INSERT_VOUCHERS = "INSERT_VOUCHERS";
                public const string DELETE_VOUCHERS = "DELETE_VOUCHERS";
            }
        }

        #region Statistic error message
        public struct StatisticMessage
        {
            public const string PROMO_COUNT_ERR = "Promotion statistic error, please try again.";
            public const string BRAND_ID_INVALID = "Brand invalid, please try again.";
        }
        #endregion
    }
}
