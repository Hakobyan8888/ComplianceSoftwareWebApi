namespace ComplianceSoftwareWebSite.Services
{
    public class TokenService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetToken(string token)
        {
            var context = _httpContextAccessor.HttpContext;
            if (context.Session != null)
            {
                context.Session.SetString("AuthToken", token);
            }
        }

        public string GetToken()
        {
            var context = _httpContextAccessor.HttpContext;
            return context.Session?.GetString("AuthToken");
        }

        public void ClearToken()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context.Session != null)
            {
                context.Session.Remove("AuthToken");
            }
        }
    }
}
