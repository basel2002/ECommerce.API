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

        // ✅ 1. CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: MyAllowSpecificOrigins,
                policy =>
                {
                    policy.WithOrigins("http://chrome.com",
                                       "http://www.contoso.com");
                });
        });

        // ✅ 2. Controllers — only ONCE, merged together
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

        // ✅ 3. Identity — before DAL (DbContext is inside it)
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        // ✅ 4. DAL & BLL
        builder.Services.AddDALServices(builder.Configuration);
        builder.Services.AddBLLServices();

        // ✅ 5. JWT Settings
        builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

        // ✅ 6. Authentication
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

        // ✅ 7. Authorization — fixed closing bracket was }; should be });
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireRole("admin"));
            options.AddPolicy("UserOnly", policy =>
                policy.RequireRole("user"));

        });

        // ✅ 8. OpenAPI
        builder.Services.AddOpenApi();
        // Add Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        var app = builder.Build();

        // ✅ Middleware pipeline — ORDER MATTERS
        if (app.Environment.IsDevelopment())
        {
            //app.MapOpenApi();
            //app.MapScalarApiReference();
            app.UseSwagger();
            app.UseSwaggerUI();

        }

        app.UseHttpsRedirection();  // 1st
        app.UseCors(MyAllowSpecificOrigins);  // 2nd ← moved BEFORE auth
        app.UseAuthentication();    // 3rd
        app.UseAuthorization();     // 4th
        app.MapControllers();       // 5th
        app.Run();
    }
}