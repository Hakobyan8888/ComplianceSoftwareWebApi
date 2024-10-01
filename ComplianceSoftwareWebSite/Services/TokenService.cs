using ComplianceSoftwareWebSite.Services.Interfaces;

namespace ComplianceSoftwareWebSite.Services
{
    public class TokenService : ITokenService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task SetToken(string token)
        {
            var context = _httpContextAccessor.HttpContext;
            if (context != null)
            {
                context.Session.SetString("AuthToken", token);
                await context.Session.CommitAsync();
            }
        }

        public string GetToken()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context != null && context.Session != null)
            {
                return context.Session.GetString("AuthToken");

            }
            return string.Empty;
        }

        public void ClearToken()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context?.Session != null)
            {
                context.Session.Remove("AuthToken");
            }
        }
    }
}
