namespace ComplianceSoftwareWebSite.Services.Interfaces
{
    public interface ITokenService
    {
        void SetToken(string token);
        string GetToken();
        void ClearToken();
    }
}
