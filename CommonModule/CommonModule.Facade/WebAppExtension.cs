using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using CommonModule.Core.Extensions;
using CommonModule.Interfaces;
using CommonModule.Repositories;
using CommonModule.Repositories.Builders;
using CommonModule.Shared.Constants;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace CommonModule.Facade;

public static class WebAppExtension
{
    #region Auth
    public static void AddGoogleAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddGoogle(options =>
            {
                options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
            });
    }

    public static void AddJwt(this WebApplicationBuilder builder)
    {
        byte[] key = builder.Configuration["Authentication:Jwt:SecretKey"].StringToUtf8Bytes();

        builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
    }

    public static void UseTokenValidator(this IApplicationBuilder app)
    {
        app.Use(async (context, next) =>
        {
            try
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    var tokenService = context.RequestServices.GetRequiredService<ITokenRepository>();
                    var token = context.Request.Headers["Authorization"].ToString().Split(' ').Last();

                    if (!await tokenService.IsTokenValidAsync(token))
                    {
                        var tokenFactory = context.RequestServices.GetRequiredService<IJwtTokenFactory>();

                        if (tokenService.IsTokenExpired(token) && tokenFactory.IsTokenRefreshable(token))
                        {
                            var newToken = tokenFactory.GenerateNewJwtToken(context.User);
                            context.Response.Headers.Add("Authorization", $"JwtHonk {newToken}");
                        }
                        else
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync(ErrorMessages.InternalServerError);
                return;
            }

            await next();
        });
    }
    #endregion

    #region Cors
    public static void AddCors(this WebApplicationBuilder builder)
    {
        string allowedHosts = builder.Configuration["AllowedHosts"];
        
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigins",
                builder =>
                {
                    builder.WithOrigins(allowedHosts)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials(); // If cookies or authorization headers are needed
                });
        });
    }
    #endregion

    #region Databases
    public static void AddDatabaseContext<TDataContext>(this WebApplicationBuilder builder, string dbName = "Database")
        where TDataContext : DbContext
    {
        builder.Services.AddDbContext<TDataContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString(dbName)));
    }
    
    public static void AddDynamoDB(this WebApplicationBuilder builder)
    {
        var dynamoDbConfig = new AmazonDynamoDBConfig
        {
            ServiceURL = builder.Configuration["DynamoDB:ServiceURL"]
        };
        
        builder.Services.AddSingleton<IAmazonDynamoDB>(sp => new AmazonDynamoDBClient(dynamoDbConfig));
        builder.Services.AddSingleton<IDynamoDBContext, DynamoDBContext>();
    }
    #endregion

    #region DependencyInjection
    public static void AddDependencyInjection(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddScoped(typeof(FilterBuilder<,>));
        builder.Services.AddScoped(typeof(IEntityValidator<>), typeof(EntityValidator<>));

        // Register Generic Repository with TDataContext
        builder.Services.AddScoped(typeof(IReadGenericRepository<,,>), typeof(GenericRepository<,,>));
        builder.Services.AddScoped(typeof(IGenericRepository<,,>), typeof(GenericRepository<,,>));

        builder.Services.AddScoped<IAuthRepository, AuthRepository>();
        builder.Services.AddScoped<IJwtTokenFactory, JwtTokenFactory>();

        builder.Services.AddScoped<ITokenRepository, DynamoDbTokenRepository>();
        builder.Services.AddScoped<ILocalizationRepository, LocalizationRepository>();
    }
    #endregion

    #region Swagger
    public static void AddSwagger(this WebApplicationBuilder builder)
    {
        string version = builder.Configuration["Microservice:Version"];
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(version,
                new OpenApiInfo { Title = builder.Configuration["Microservice:Title"], Version = version });
            c.AddSecurityDefinition("JwtHonk", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the JwtHonk scheme. \r\n\r\n 
                      Enter 'JwtHonk' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'JwtHonk 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "JwtHonk"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "JwtHonk"
                        },
                        Scheme = "oauth2",
                        Name = "JwtHonk",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
            // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            // c.IncludeXmlComments(xmlPath);
        });
    }
    #endregion
}