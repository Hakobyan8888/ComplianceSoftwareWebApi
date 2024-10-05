namespace ComplianceSoftwareWebSite.Models
{
    public class CompanyModel
    {
        public string EntityType { get; set; } = string.Empty;
        public string StateOfFormation { get; set; } = string.Empty;
        public string BusinessName { get; set; } = string.Empty;
        public IndustryType BusinessIndustry { get; set; }
        public string ZipCode { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string StreetAddress { get; set; } = string.Empty;
        public DateTime? EntityFormationDate { get; set; }
    }

    public class IndustryType
    {
        public IndustryType(int industryTypeCode, string industryName)
        {
            IndustryTypeCode = industryTypeCode;
            IndustryName = industryName;
        }
        public int IndustryTypeCode { get; set; }
        public string IndustryName { get; set; }
    }
}
