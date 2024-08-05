using IdentityWebApiSample.Server.AppSettings;
using IdentityWebApiSample.Server.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using IdentityWebApiSample.Server.DataContext;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using IdentityWebApiSample.Server.Services;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;
using IdentityWebApiSample.Server.Dtos.Login;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// settings
var postgreSetting = new PostgreSettings();
builder.Configuration.Bind("PostgreSetting", postgreSetting);
builder.Services.AddSingleton(postgreSetting);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(postgreSetting.ConnectionString));

// Identity
builder.Services.AddIdentity<UserSystem, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Add JWT Bearer authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidAudience = configuration["JWT:ValidAudience"],
            ValidIssuer = configuration["JWT:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
        };
    });

//Add authentication to Swagger UI
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin", policyOption =>
    {
        policyOption.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddApplicationServicesExtension();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromHours(10));

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseCors("AllowOrigin");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthentication();
app.UseCookiePolicy();
app.MapControllers();

app.Run();