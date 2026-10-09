namespace bexio_lib.Models
{
    public class BexioNote
    {
        public int? id { get; set; }
        public int? user_id { get; set; }
        public string event_start { get; set; }
        public string subject { get; set; }
        public string info { get; set; }
        public int? contact_id { get; set; }
        public int? project_id { get; set; }
        public int? entry_id { get; set; }
        public int? module_id { get; set; }
    }

    public class BexioTask
    {
        public int? id { get; set; }
        public int? user_id { get; set; }
        public string finish_date { get; set; }
        public string subject { get; set; }
        public int? place_id { get; set; }
        public string info { get; set; }
        public int? contact_id { get; set; }
        public int? sub_contact_id { get; set; }
        public int? project_id { get; set; }
        public int? entry_id { get; set; }
        public int? module_id { get; set; }
        public int? todo_status_id { get; set; }
        public int? todo_priority_id { get; set; }
        public bool? has_reminder { get; set; }
        public string reminder_type_id { get; set; }
        public int? reminder_value { get; set; }
        public string remember_type_id { get; set; }
        public int? remember_time_id { get; set; }
        public int? communication_kind_id { get; set; }
    }

    public class BexioProject
    {
        public int? id { get; set; }
        public string uuid { get; set; }
        public string nr { get; set; }
        public string name { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string comment { get; set; }
        public int? pr_state_id { get; set; }
        public int? pr_project_type_id { get; set; }
        public int? contact_id { get; set; }
        public int? contact_sub_id { get; set; }
        public int? pr_invoice_type_id { get; set; }
        public string pr_invoice_type_amount { get; set; }
        public int? pr_budget_type_id { get; set; }
        public string pr_budget_type_amount { get; set; }
        public int? user_id { get; set; }
    }

    public class BexioTimesheet
    {
        public int? id { get; set; }
        public int? user_id { get; set; }
        public int? status_id { get; set; }
        public int? client_service_id { get; set; }
        public string text { get; set; }
        public bool? allowable_bill { get; set; }
        public object charge { get; set; }
        public int? contact_id { get; set; }
        public int? sub_contact_id { get; set; }
        public int? pr_project_id { get; set; }
        public int? pr_package_id { get; set; }
        public int? pr_milestone_id { get; set; }
        public string travel_time { get; set; }
        public object travel_charge { get; set; }
        public object travel_distance { get; set; }
        public string estimated_time { get; set; }
        public string date { get; set; }
        public string duration { get; set; }
        public bool? running { get; set; }
        public BexioTimesheetTracking tracking { get; set; }
    }

    /// <summary>
    /// Either type = "duration" with date + duration ("HH:mm") or type = "range" with start / end
    /// </summary>
    public class BexioTimesheetTracking
    {
        public string type { get; set; }
        public string date { get; set; }
        public string duration { get; set; }
        public string start { get; set; }
        public string end { get; set; }
    }
}
