using System.ComponentModel.DataAnnotations;

namespace ComplianceSoftwareWebApi.Models
{
    public class IndustryType
    {
        [Key]
        public int Id { get; set; }
        public IndustryType(int industryTypeCode, string industryName)
        {
            IndustryTypeCode = industryTypeCode;
            IndustryName = industryName;
        }
        public int IndustryTypeCode { get; set; }
        public string IndustryName { get; set; }
    }
}
