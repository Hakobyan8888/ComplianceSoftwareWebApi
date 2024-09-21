using ComplianceSoftwareWebApi.DTOs;
using ComplianceSoftwareWebApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComplianceSoftwareWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly IUserService _userService;
        public CompanyController(ICompanyService companyService, IUserService userService)
        {
            _companyService = companyService;
            _userService = userService;
        }
        [Authorize(Roles = "Owner")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyDto companyDto)
        {
            var userId = _userService.GetUserIdFromClaims(User);
            var company = await _companyService.AddCompanyAsync(companyDto, userId);
            return Ok(company);
        }
    }
}