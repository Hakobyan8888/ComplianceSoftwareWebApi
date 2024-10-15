using ComplianceSoftwareWebApi.DTOs;
using ComplianceSoftwareWebApi.Models;
using ComplianceSoftwareWebApi.Services.Interfaces;
using ComplianceSoftwareWebApi.UnitOfWork;

namespace ComplianceSoftwareWebApi.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Company> AddCompanyAsync(CompanyDto registerDto, string userId)
        {
            try
            {
                // Create a new company entity from the DTO
                var company = new Company()
                {
                    StreetAddress = registerDto.StreetAddress,
                    BusinessName = registerDto.BusinessName,
                    BusinessIndustryCode = registerDto.BusinessIndustryCode,
                    City = registerDto.City,
                    EntityType = registerDto.EntityType,
                    StateOfFormation = registerDto.StateOfFormation,
                    ZipCode = registerDto.ZipCode,
                };

                // Retrieve the user by userId
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    throw new ArgumentException("User not found");
                }

                // If the user is an owner and no user permissions exist, assign all permissions
                if (user.Role == Roles.Owner && !(await _unitOfWork.UserPermissions.GetAllAsync()).Any())
                {
                    var permissions = await _unitOfWork.Permissions.GetAllAsync();
                    var userPermissions = new List<UserPermission>();

                    foreach (var permission in permissions)
                    {
                        var userPermission = new UserPermission
                        {
                            Permission = permission,
                            UserId = userId
                        };
                        userPermissions.Add(userPermission);
                        await _unitOfWork.UserPermissions.AddAsync(userPermission);
                    }
                }

                // Add the new company
                await _unitOfWork.Companies.AddAsync(company);
                await _unitOfWork.CompleteAsync();

                // Update the user with the new company Id
                user.CompanyId = company.Id;
                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.CompleteAsync();

                return company;
            }
            catch (ArgumentException ex)
            {
                // Handle known errors
                throw;
            }
            catch (Exception)
            {
                // Let middleware handle unexpected exceptions
                throw;
            }
        }

        public async Task<Company> GetCompany(string userId)
        {
            try
            {
                // Retrieve the user by userId
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    throw new ArgumentException("User not found");
                }

                // Check if the user is associated with a company
                if (user.CompanyId == null)
                {
                    return null;
                }

                // Retrieve the company by Id
                return await _unitOfWork.Companies.GetByIdAsync(Convert.ToInt32(user.CompanyId));
            }
            catch (ArgumentException ex)
            {
                // Handle known errors
                throw;
            }
            catch (Exception)
            {
                // Let middleware handle unexpected exceptions
                throw;
            }
        }

        public async Task<List<Industry>> GetIndustries()
        {
            try
            {
                // Retrieve all industry types
                return (await _unitOfWork.Industries.GetAllAsync()).ToList();
            }
            catch (Exception)
            {
                // Let middleware handle unexpected exceptions
                throw;
            }
        }


        public Task<List<string>> GetEntityTypes()
        {
            return Task.FromResult(new List<string>
            {
                "Sole Proprietorship", "Partnership", "Corporation (C-Corp)", "S Corporation (S-Corp)",
                "Limited Liability Company (LLC)", "Nonprofit Corporation", "Cooperative (Co-op)",
                "Professional Corporation (PC)", "Benefit Corporation (B-Corp)", "Joint Venture",
                "Trust", "Private Limited Company (Ltd)", "Public Limited Company (PLC)",
                "Holding Company", "Series LLC"
            });
        }

        public async Task<IEnumerable<License>> GetRequiredLicenses(string userId)
        {
            var company = await GetCompany(userId);
            if (company == null)
            {
                throw new ArgumentException("Company not found");
            }
            var industry = await _unitOfWork.Industries.GetIndustryLicenses(company.BusinessIndustryCode);
            var licenses = await _unitOfWork.Licenses.GetLicensesByIndustryCode(industry.Id);
            return licenses;
        }
    }
}
