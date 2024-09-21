using ComplianceSoftwareWebApi.Models;

namespace ComplianceSoftwareWebApi.Services.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user, List<string> roles);
    }

}
