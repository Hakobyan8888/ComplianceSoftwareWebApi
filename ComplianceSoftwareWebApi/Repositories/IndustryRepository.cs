using ComplianceSoftwareWebApi.Data;
using ComplianceSoftwareWebApi.Models;
using ComplianceSoftwareWebApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ComplianceSoftwareWebApi.Repositories
{
    public class IndustryRepository : Repository<Industry>, IIndustryRepository
    {
        public IndustryRepository(ApplicationDbContext context) : base(context)
        {
        }

        // Custom methods for Industry repository

        public async Task<Industry> GetIndustryLicenses(int id)
        {
            return await _context.Industries.FirstOrDefaultAsync(x => x.IndustryCode == id);//Include(x => x.Licenses).FirstOrDefaultAsync(i => i.IndustryCode == id);
        }
    }
}
