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
                Address = registerDto.Address,
                Name = registerDto.Name,
                Industry = registerDto.Industry,
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
                        Id = id++,
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
    }
}
