using ComplianceSoftwareWebApi.Data;
using ComplianceSoftwareWebApi.Models;
using ComplianceSoftwareWebApi.Repositories.Interfaces;

namespace ComplianceSoftwareWebApi.Repositories
{
    public class IndustryTypeRepository : Repository<IndustryType>, IIndustryTypeRepository
    {
        public IndustryTypeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
