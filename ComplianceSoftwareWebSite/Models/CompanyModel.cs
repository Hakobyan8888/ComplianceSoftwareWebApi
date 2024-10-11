namespace ComplianceSoftwareWebSite.Models
{
    public class CompanyModel
    {
        public string EntityType { get; set; } = string.Empty;
        public string StateOfFormation { get; set; } = string.Empty;
        public string BusinessName { get; set; } = string.Empty;
        public Industry BusinessIndustry { get; set; }
        public string ZipCode { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string StreetAddress { get; set; } = string.Empty;
        public DateTime? EntityFormationDate { get; set; }
    }

    public class Industry
    {
        public Industry(int industryTypeCode, string industryName, int industryCode, string industryType)
        {
            IndustryTypeCode = industryTypeCode;
            IndustryName = industryName;
            IndustryCode = industryCode;
            IndustryType = industryType;
        }
        public int IndustryTypeCode { get; set; }
        public string IndustryName { get; set; }

        public int IndustryCode { get; set; }

        public string IndustryType { get; set; }
    }
}
