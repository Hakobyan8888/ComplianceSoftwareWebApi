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

        /// <summary>
        /// Creates a new company. Only accessible to users with the Owner role.
        /// </summary>
        /// <param name="companyDto">The company data to create.</param>
        /// <returns>Returns the created company or an appropriate error message.</returns>
        [Authorize(Roles = "Owner")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyDto companyDto)
        {
            try
            {
                // Validate input
                if (companyDto == null)
                {
                    return BadRequest("Company data cannot be null.");
                }

                var userId = _userService.GetUserIdFromClaims(User);
                var company = await _companyService.AddCompanyAsync(companyDto, userId);

                return CreatedAtAction(nameof(GetCompany), new { id = company.Id }, company); // Returns 201 Created
            }
            catch (Exception ex)
            {
                // Let global middleware handle unhandled exceptions
                throw;
            }
        }

        /// <summary>
        /// Retrieves the company associated with the current user.
        /// </summary>
        /// <returns>Returns the company data or a 404 if no company is found.</returns>
        [Authorize]
        [HttpGet("get")]
        public async Task<IActionResult> GetCompany()
        {
            try
            {
                var userId = _userService.GetUserIdFromClaims(User);
                var company = await _companyService.GetCompany(userId);

                if (company == null)
                {
                    return NotFound("No company found for the user.");
                }

                return Ok(company);
            }
            catch (Exception ex)
            {
                // Let global middleware handle unhandled exceptions
                throw;
            }
        }

        /// <summary>
        /// Retrieves the list of entity types. Only accessible to users with the Owner role.
        /// </summary>
        /// <returns>Returns the list of entity types.</returns>
        [Authorize(Roles = "Owner")]
        [HttpGet("entity-types")]
        public async Task<IActionResult> GetEntityTypes()
        {
            try
            {
                var entityTypes = await _companyService.GetEntityTypes();

                if (entityTypes == null || !entityTypes.Any())
                {
                    return NotFound("No entity types found.");
                }

                return Ok(JsonSerializer.Serialize(entityTypes));
            }
            catch (Exception ex)
            {
                // Let global middleware handle unhandled exceptions
                throw;
            }
        }

        /// <summary>
        /// Retrieves the list of industries. Only accessible to users with the Owner role.
        /// </summary>
        /// <returns>Returns the list of industries.</returns>
        [Authorize(Roles = "Owner")]
        [HttpGet("industries")]
        public async Task<IActionResult> GetIndustries()
        {
            try
            {
                var industries = await _companyService.GetIndustries();

                if (industries == null || !industries.Any())
                {
                    return NotFound("No industries found.");
                }

                return Ok(JsonSerializer.Serialize(industries));
            }
            catch (Exception ex)
            {
                // Let global middleware handle unhandled exceptions
                throw;
            }
        }



    }
}