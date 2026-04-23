using BLL;
using DAL;
using DAL.Data.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

public class Program
{
    public static void Main(string[] args)
    {
        var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        var builder = WebApplication.CreateBuilder(args);

        // CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: MyAllowSpecificOrigins,
                policy =>
                {
                    policy.WithOrigins("http://chrome.com",
                                       "http://www.contoso.com");
                });
        });

        //  Controllers 
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

        // Identity 
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        // DAL & BLL
        builder.Services.AddDALServices(builder.Configuration);
        builder.Services.AddBLLServices();

        // JWT Settings
        builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

        //  Authentication
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
                ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                ValidAudience = builder.Configuration["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Convert.FromBase64String(builder.Configuration["JwtSettings:SecretKey"]))
            };
        });

        //  Authorization 
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireRole("admin"));
            options.AddPolicy("UserOnly", policy =>
                policy.RequireRole("user"));

        });

        //  OpenAPI
        builder.Services.AddOpenApi();



        var app = builder.Build();

        //  Middleware pipeline — 
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();


        }

        app.UseHttpsRedirection();  
        app.UseCors(MyAllowSpecificOrigins);  
        app.UseAuthentication();    
        app.UseAuthorization();     
        app.MapControllers();       
        app.Run();
    }
}