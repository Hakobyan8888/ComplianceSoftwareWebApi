using ComplianceSoftwareWebApi.Data;
using ComplianceSoftwareWebApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace ComplianceSoftwareWebApi.Repositories
{
    public class LicenseRepository : Repository<License>, ILicenseRepository
    {
        public LicenseRepository(ApplicationDbContext context) : base(context)
        {
        }

        // Custom methods for License repository
    }
}
