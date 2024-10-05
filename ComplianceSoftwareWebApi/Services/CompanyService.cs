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
            var company = new Company()
            {
                StreetAddress = registerDto.StreetAddress,
                BusinessName = registerDto.BusinessName,
                BusinessIndustryCode = registerDto.BusinessIndustry.IndustryTypeCode,
                City = registerDto.City,
                EntityType = registerDto.EntityType,
                StateOfFormation = registerDto.StateOfFormation,
                ZipCode = registerDto.ZipCode,
            };

            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user.Role == Roles.Owner && !(await _unitOfWork.UserPermissions.GetAllAsync()).Any())
            {
                var permissions = await _unitOfWork.Permissions.GetAllAsync();
                var userPermissions = new List<UserPermission>();
                int id = 0;
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
            await _unitOfWork.Companies.AddAsync(company);
            await _unitOfWork.CompleteAsync();

            user.CompanyId = company.Id;
            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.CompleteAsync();
            return company;
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

        public async Task<List<IndustryType>> GetIndustries()
        {
            return (await _unitOfWork.IndustryTypes.GetAllAsync()).ToList();
        }
    }
}
