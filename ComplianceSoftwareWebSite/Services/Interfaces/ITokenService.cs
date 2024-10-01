namespace ComplianceSoftwareWebSite.Services.Interfaces
{
    public interface ITokenService
    {
        Task SetToken(string token);
        string GetToken();
        void ClearToken();
    }
}
