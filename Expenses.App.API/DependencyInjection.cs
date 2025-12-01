using Expenses.App.API.Database;
using Expenses.App.API.Database.Schema;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.OpenApi.Models;
using Npgsql;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Microsoft.IdentityModel.Tokens;
using Expenses.App.API.Settings;
using System.Text;
using Expenses.App.API.Services;
using FluentValidation;
using Expenses.App.API.Middleware;
using Expenses.App.API.Services.Sorting;
using Expenses.App.API.Dtos.Categories;
using Expenses.App.API.Models;
using Expenses.App.API.Dtos.Categories.Mappings;

namespace Expenses.App.API;

public static class DependencyInjection
{
    // ADD CORS POLICY ---------------------

    public static WebApplicationBuilder AddCorsPolicy(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(
              opt => opt.AddPolicy("AllowAll",
              opt => opt.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin()
              )
        );

        return builder;
    }

    // ADD SERVICE INTEGRATION SUPPORTS ---------------------

    public static WebApplicationBuilder AddServiceIntegrationSupports(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddValidatorsFromAssemblyContaining<Program>();
        builder.Services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
            };
        });

        // global exception handler ---------------------

        builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

        return builder;
    }

    // REGISTER APPLICATION SERVICE ---------------------

    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        // token

        builder.Services.AddTransient<TokenProvider>();

        // mapping sort

        builder.Services.AddTransient<SortMappingProvider>();

        builder.Services.AddSingleton<ISortMappingDefinition, SortMappingDefinition<CategoryDto, Category>>(_ => CategoryMapping.SortMapping);

        // memory cache

        builder.Services.AddMemoryCache();
        builder.Services.AddScoped<UserContext>();

        return builder;
    }

    // OPEN TELEMETRY ---------------------

    public static WebApplicationBuilder AddObservability(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenTelemetry()
                        .ConfigureResource(resources => resources.AddService(builder.Environment.ApplicationName))
                        .WithTracing(tracing => tracing
                            .AddHttpClientInstrumentation()
                            .AddAspNetCoreInstrumentation()
                            .AddNpgsql())
                        .WithMetrics(metrics => metrics
                            .AddHttpClientInstrumentation()
                            .AddAspNetCoreInstrumentation()
                            .AddRuntimeInstrumentation())
                        .UseOtlpExporter();

        builder.Logging.AddOpenTelemetry(options =>
        {
            options.IncludeScopes = true;
            options.IncludeFormattedMessage = true;
        });

        return builder;
    }

    // DATABASE ---------------------

    public static WebApplicationBuilder AddDatabase(this WebApplicationBuilder builder)
    {
        //  application migrations

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(
                        builder.Configuration.GetConnectionString("Database"),
                        npgsqlOptions => npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Application)).UseSnakeCaseNamingConvention();
        });

        // identity migration

        builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
        {
            options.UseNpgsql(
                        builder.Configuration.GetConnectionString("Database"),
                        npgsqlOptions => npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Identity)).UseSnakeCaseNamingConvention();
        });

        return builder;
    }

    //  AUTHENTICATION & JWT BEARER ---------------------

    public static WebApplicationBuilder AddAuthenticationServices(this WebApplicationBuilder builder)
    {
        // add identity

        builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                        .AddEntityFrameworkStores<ApplicationIdentityDbContext>();

        // jwt bearer

        builder.Services.Configure<JwtAuthOptions>(builder.Configuration.GetSection("Jwt"));

        JwtAuthOptions jwtAuthOptions = builder.Configuration.GetSection("Jwt").Get<JwtAuthOptions>()!;

        builder.Services
                    .AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidIssuer = jwtAuthOptions.Issuer,
                            ValidAudience = jwtAuthOptions.Audience,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAuthOptions.Key))
                        };
                    });

        // authorization

        builder.Services.AddAuthorization();

        return builder;
    }

    // SWAGGER ---------------------

    public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(configuration =>
        {
            configuration.SwaggerDoc("v1", new OpenApiInfo { Title = "Expenses API", Version = "v1" });

            configuration.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter 'Bearer' [space] and then your valid token.\r\n\r\nExample: \"Bearer eyJhbGciOi...\""
            });

            configuration.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return builder;
    }
}
