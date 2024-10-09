using ComplianceSoftwareWebApi.Data;
using ComplianceSoftwareWebApi.Services.Interfaces;
using ComplianceSoftwareWebApi.Services;
using ComplianceSoftwareWebApi.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using ComplianceSoftwareWebApi.Models;
using ComplianceSoftwareWebApi.Repositories.Interfaces;
using ComplianceSoftwareWebApi.Repositories;
using System.Security.Claims;
using ComplianceSoftwareWebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);
//builder.WebHost.UseUrls("http://localhost:8080", "https://localhost:9090");

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;

    // User settings
    options.User.RequireUniqueEmail = true;
});

// Other services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        RoleClaimType = ClaimTypes.Role,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await InitializeRoles(services); // Call the role initialization method
    await InitializePermissions(services);
    await InitializeIndustryTypes(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();




async Task InitializeRoles(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    List<string> roleNames = Enum.GetValues(typeof(Roles))
        .Cast<Roles>()
        .Select(r => r.ToString())
        .ToList();
    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}

async Task InitializePermissions(IServiceProvider serviceProvider)
{
    var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
    var permissionsDB = (await unitOfWork.Permissions.GetAllAsync()).ToList();
    List<int> permissions = Enum.GetValues(typeof(PermissionTypes))
       .Cast<PermissionTypes>()
       .Select(r => (int)r)
       .ToList();

    foreach (var permission in permissions)
    {
        if (!permissionsDB.Any(x => (int)x.PermissionType == permission))
        {
            await unitOfWork.Permissions.AddAsync(new Permission() { PermissionType = (PermissionTypes)permission });
        }
    }
    await unitOfWork.CompleteAsync();
}

async Task InitializeIndustryTypes(IServiceProvider serviceProvider)
{
    var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
    var industriesDB = (await unitOfWork.IndustryTypes.GetAllAsync()).ToList();
    List<IndustryType> industries = new List<IndustryType>
            {
                new IndustryType(11, "Agriculture, Forestry, Fishing and Hunting"),
                new IndustryType(21, "Mining, Quarrying, and Oil and Gas Extraction"),
                new IndustryType(22, "Utilities"),
                new IndustryType(23, "Construction"),
                new IndustryType(31, "Manufacturing"),
                new IndustryType(42, "Wholesale Trade"),
                new IndustryType(44, "Retail Trade"),
                new IndustryType(48, "Transportation and Warehousing"),
                new IndustryType(51, "Information"),
                new IndustryType(52, "Finance and Insurance"),
                new IndustryType(53, "Real Estate and Rental and Leasing"),
                new IndustryType(54, "Professional, Scientific, and Technical Services"),
                new IndustryType(55, "Management of Companies and Enterprises"),
                new IndustryType(56, "Administrative and Support and Waste Management and Remediation Services"),
                new IndustryType(61, "Educational Services"),
                new IndustryType(62, "Health Care and Social Assistance"),
                new IndustryType(71, "Arts, Entertainment, and Recreation"),
                new IndustryType(72, "Accommodation and Food Services"),
                new IndustryType(81, "Other Services (except Public Administration)"),
                new IndustryType(92, "Public Administration"),
            };

    foreach (var industry in industries)
    {
        if (!industriesDB.Any(x => (int)x.IndustryTypeCode == industry.IndustryTypeCode))
        {
            await unitOfWork.IndustryTypes.AddAsync(industry);
        }
    }
    await unitOfWork.CompleteAsync();
}