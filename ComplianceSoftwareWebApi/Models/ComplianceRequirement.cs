namespace ComplianceSoftwareWebApi.Models
{
    public class ComplianceRequirement
    {
        public int Id { get; set; }
        public string Permit { get; set; }
        public string License { get; set; }
        public string Regulation { get; set; }
        public string Jurisdiction { get; set; }
    }

}
