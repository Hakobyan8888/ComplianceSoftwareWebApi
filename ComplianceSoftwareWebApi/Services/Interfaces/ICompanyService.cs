﻿using ComplianceSoftwareWebApi.DTOs;
using ComplianceSoftwareWebApi.Models;

namespace ComplianceSoftwareWebApi.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<Company> AddCompanyAsync(CompanyDto registerDto, string userId);
    }
}