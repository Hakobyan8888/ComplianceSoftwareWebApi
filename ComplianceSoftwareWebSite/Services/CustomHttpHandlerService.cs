
namespace ComplianceSoftwareWebSite.Services
{
    public class CustomHttpHandlerService : DelegatingHandler
    {
        private readonly TokenService _tokenService;
        public CustomHttpHandlerService(TokenService tokenService)
        {
            _tokenService = tokenService;
        }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = _tokenService.GetToken();
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return base.SendAsync(request, cancellationToken);
        }
    }
}
