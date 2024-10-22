using ComplianceSoftwareWebApi.Data;
using ComplianceSoftwareWebApi.Models;
using ComplianceSoftwareWebApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ComplianceSoftwareWebApi.Repositories
{
    public class LicenseRepository : Repository<License>, ILicenseRepository
    {
        public LicenseRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<License>> GetLicensesByIndustryCode(int code)
        {
            return await _context.Licenses.Where(x => x.IndustryId == code && string.IsNullOrEmpty(x.StateCode)).ToListAsync();
        }

        public async Task<IEnumerable<License>> GetLicensesByIndustryCodeAndState(int code, string stateCode)
        {
            return await _context.Licenses.Where(x => x.IndustryId == code && x.StateCode.ToLower() == stateCode.ToLower()).ToListAsync();
        }

        // Custom methods for License repository
    }
}
