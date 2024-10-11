using ExcelSqlHelper.Models;
using OfficeOpenXml;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ExcelSqlHelper
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Required by EPPlus for non-commercial use

            string filePath = @"C:\Users\haste\Downloads\Cleaned_Federal_Licenses by 550 industries AM .xlsx";

            var industries = ReadIndustryDataFromExcel(filePath);
            await SaveIndustriesToDatabaseAsync(industries);
        }

        static List<Industry> ReadIndustryDataFromExcel(string filePath)
        {
            var industries = new List<Industry>();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                if (worksheet == null) return industries;

                int rowCount = worksheet.Dimension.Rows;
                for (int row = 2; row <= rowCount; row++) // Assuming first row is headers
                {
                    var industry = new Industry
                    {
                        IndustryType = worksheet.Cells[row, 2].Text,
                        IndustryCode = int.Parse(worksheet.Cells[row, 3].Text),
                        SectorCode = worksheet.Cells[row, 4].Text,
                        IndustryName = worksheet.Cells[row, 5].Text
                    };

                    var license1 = worksheet.Cells[row, 6].Text;
                    var issuingAgency1 = worksheet.Cells[row, 7].Text;
                    var license2 = worksheet.Cells[row, 8].Text;
                    var issuingAgency2 = worksheet.Cells[row, 9].Text;
                    var license3 = worksheet.Cells[row, 10].Text;
                    var issuingAgency3 = worksheet.Cells[row, 11].Text;

                    if (!string.IsNullOrEmpty(license1) && !license1.Equals("N/A") && !license1.Equals("None") && !license1.Equals("No federal license is required"))
                    {
                        industry.Licenses.Add(new License { LicenseName = license1, IssuingAgency = issuingAgency1, IssuingAgencyLink="" });
                    }
                    if (!string.IsNullOrEmpty(license2) && !license2.Equals("N/A") && !license2.Equals("None") && license2.Equals("No federal license is required"))
                    {
                        industry.Licenses.Add(new License { LicenseName = license2, IssuingAgency = issuingAgency2, IssuingAgencyLink = "" });
                    }
                    if (!string.IsNullOrEmpty(license3) && !license3.Equals("N/A") && !license3.Equals("None") && license3.Equals("No federal license is required"))
                    {
                        industry.Licenses.Add(new License { LicenseName = license3, IssuingAgency = issuingAgency3, IssuingAgencyLink = "" });
                    }

                    industries.Add(industry);
                }
            }

            return industries;
        }

        static async Task SaveIndustriesToDatabaseAsync(List<Industry> industries)
        {
            using (var context = new ApplicationDbContext())
            {
                foreach (var industry in industries)
                {
                    context.Industries.Add(industry);
                }

                await context.SaveChangesAsync();
                Console.WriteLine("Industries and licenses have been saved to the database.");
            }
        }
    }
}
