using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bexio_lib.Data
{
    public static class BexioFilterCriteria
    {
        public const string EXACT_MATCH = "equal";
        public const string NOT_EQUAL = "not_equal";
        public const string GREATER_THAN = "greater_than";
        public const string LESS_THAN = "less_than";
        public const string GREATER_EQUAL = "greater_equal";
        public const string LESS_EQUAL = "less_equal";
        public const string LIKE = "like";
        public const string NOT_LIKE = "not_like";
        public const string IS_NULL = "is_null";
        public const string IN = "in";
        public const string NOT_IN = "not_in";
    }

    public static class BexioPositionType
    {
        public const string KbPositionArticle = "KbPositionArticle";
        public const string KbPositionCustom = "KbPositionCustom";
        public const string KbPositionText = "KbPositionText";
        public const string KbPositionSubposition = "KbPositionSubposition";
        public const string KbPositionSubtotal = "KbPositionSubtotal";
        public const string KbPositionPagebreak = "KbPositionPagebreak";
        public const string KbPositionDiscount = "KbPositionDiscount";
    }

    public static class BexioQuoteStatus
    {
        public const int DRAFT = 1;
        public const int PENDING = 2;
        public const int CONFIRMED = 3;
        public const int DECLINED = 4;
    }

    public static class BexioOrderStatus
    {
        public const int PENDING = 5;
        public const int DONE = 6;
        public const int PARTIAL = 15;
        public const int CANCELED = 21;
    }

    public static class BexioInvoiceStatus
    {
        public const int DRAFT = 7;
        public const int PENDING = 8;
        public const int PAID = 9;
        public const int PARTIAL = 16;
        public const int CANCELED = 19;
        public const int UNPAID = 31;
    }

    public static class BexioContactTypes
    {
        public const int COMPANY = 1;
        public const int PERSON = 2;
    }

    public static class BexioQueryParameters
    {
        public const string LIMIT = "limit";
        public const string OFFSET = "offset";
        public const string ORDER_BY = "order_by";
    }


    public static class BexioCountryFilterFields
    {
        public const string name = "name";
        public const string name_short = "name_short";
    }

    public static class BexioCurrencyFilterFields
    {
        public const string name = "name";
    }

    public static class BexioInvoiceFilterFields
    {
        public const string id = "id";
        public const string document_nr = "document_nr";
        public const string api_reference = "api_reference";
        public const string kb_item_status_id = "kb_item_status_id";
    }

    public static class BexioItemFilterFields
    {
        public const string id = "id";
        public const string intern_code = "intern_code";
        public const string intern_name = "intern_name";
    }
}
