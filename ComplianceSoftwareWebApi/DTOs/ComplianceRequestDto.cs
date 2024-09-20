namespace ComplianceSoftwareWebApi.DTOs
{
    public class ComplianceRequestDto
    {
        public int CompanyId { get; set; }
        public string RegulationType { get; set; } // Federal, State, Local
        public string PermitName { get; set; }
    }

}
