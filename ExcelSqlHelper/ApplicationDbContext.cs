using ExcelSqlHelper.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelSqlHelper
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Industry> Industries { get; set; }
        public DbSet<License> Licenses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=ComplianceDb;Trusted_Connection=True;MultipleActiveResultSets=true;User Id=Compliance;Password=Compliance;Encrypt=True;TrustServerCertificate=True;");
        }
    }
}
