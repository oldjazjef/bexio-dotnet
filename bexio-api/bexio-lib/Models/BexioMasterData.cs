using System.Collections.Generic;

namespace bexio_lib.Models
{
    /// <summary>Contact group (contact_group)</summary>
    public class BexioContactGroup
    {
        public int? id { get; set; }
        public string name { get; set; }
    }

    /// <summary>Contact sector / branch (contact_branch)</summary>
    public class BexioContactSector
    {
        public int? id { get; set; }
        public string name { get; set; }
    }

    public class BexioSalutation
    {
        public int? id { get; set; }
        public string name { get; set; }
    }

    public class BexioTitle
    {
        public int? id { get; set; }
        public string name { get; set; }
    }

    public class BexioLanguage
    {
        public int? id { get; set; }
        public string name { get; set; }
        public string decimal_point { get; set; }
        public string thousands_separator { get; set; }
        public string date_format_id { get; set; }
        public string date_format { get; set; }
        public string iso_639_1 { get; set; }
    }

    public class BexioContactRelation
    {
        public int? id { get; set; }
        public int? contact_id { get; set; }
        public int? contact_sub_id { get; set; }
        public string description { get; set; }
        public string updated_at { get; set; }
    }

    public class BexioAdditionalAddress
    {
        public int? id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string street_name { get; set; }
        public string house_number { get; set; }
        public string address_addition { get; set; }
        public string postcode { get; set; }
        public string city { get; set; }
        public int? country_id { get; set; }
        public string subject { get; set; }
        public string description { get; set; }
    }

    public class BexioUnit
    {
        public int? id { get; set; }
        public string name { get; set; }
    }

    public class BexioPaymentType
    {
        public int? id { get; set; }
        public string name { get; set; }
    }

    public class BexioArticleType
    {
        public int? id { get; set; }
        public string name { get; set; }
    }

    public class BexioStock
    {
        public int? id { get; set; }
        public string name { get; set; }
    }

    public class BexioStockPlace
    {
        public int? id { get; set; }
        public string name { get; set; }
    }

    public class BexioClientService
    {
        public int? id { get; set; }
        public string name { get; set; }
        public float? default_is_billable { get; set; }
        public float? default_price_per_hour { get; set; }
        public float? account_id { get; set; }
    }

    public class BexioCommunicationKind
    {
        public int? id { get; set; }
        public string name { get; set; }
    }

    public class BexioTimesheetStatus
    {
        public int? id { get; set; }
        public string name { get; set; }
    }

    public class BexioProjectType
    {
        public int? id { get; set; }
        public string name { get; set; }
    }

    public class BexioProjectStatus
    {
        public int? id { get; set; }
        public string name { get; set; }
    }

    public class BexioUser
    {
        public int? id { get; set; }
        public string salutation_type { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public bool? is_superadmin { get; set; }
        public bool? is_accountant { get; set; }
    }

    public class BexioFictionalUser
    {
        public int? id { get; set; }
        public string salutation_type { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string title_id { get; set; }
    }

    public class BexioCompanyProfile
    {
        public int? id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public int? address_nr { get; set; }
        public int? postcode { get; set; }
        public string city { get; set; }
        public int? country_id { get; set; }
        public string legal_form { get; set; }
        public string country_name { get; set; }
        public string domain_name { get; set; }
        public string phone_fixed { get; set; }
        public string phone_mobile { get; set; }
        public string fax { get; set; }
        public string email { get; set; }
        public string website { get; set; }
        public string ust_id_nr { get; set; }
        public string mwst_nr { get; set; }
        public string trade_register_no { get; set; }
        public string logo_base64 { get; set; }
        public bool? is_trial { get; set; }
    }
}
