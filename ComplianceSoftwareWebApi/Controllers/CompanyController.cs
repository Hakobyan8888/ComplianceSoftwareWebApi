using ComplianceSoftwareWebApi.DTOs;
using ComplianceSoftwareWebApi.Models;
using ComplianceSoftwareWebApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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

        [Authorize]
        [HttpGet("get")]
        public async Task<IActionResult> GetCompany()
        {
            var userId = _userService.GetUserIdFromClaims(User);
            var company = await _companyService.GetCompany(userId);
            if (company == null)
            {
                return NotFound();
            }
            return Ok(company);
        }

        [Authorize(Roles = "Owner")]
        [HttpGet("entity-types")]
        public async Task<IActionResult> GetEntityTypes()
        {
            var entityTypes = await _companyService.GetEntityTypes();
            return Ok(JsonSerializer.Serialize(entityTypes));
        }

        [Authorize(Roles = "Owner")]
        [HttpGet("industries")]
        public async Task<IActionResult> GetIndustries()
        {
            var entityTypes = await _companyService.GetIndustries();
            return Ok(JsonSerializer.Serialize(entityTypes));
        }


    }
}