using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using CommonModule.Core.Filters;
using CommonModule.Interfaces;
using CommonModule.Repositories;
using CommonModule.Repositories.Builders;
using CommonModule.Shared.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using StackExchange.Redis;
using OpenApiInfo = Microsoft.OpenApi.Models.OpenApiInfo;
using OpenApiSecurityRequirement = Microsoft.OpenApi.Models.OpenApiSecurityRequirement;
using OpenApiSecurityScheme = Microsoft.OpenApi.Models.OpenApiSecurityScheme;

namespace CommonModule.Facade
{
    public static class WebAppExtension
    {
        #region Auth

        public static void AddAuthorization(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthorization();
            // builder.Services.AddAuthorization(options =>
            // {
            //     options.DefaultPolicy = new AuthorizationPolicyBuilder(AuthSchema.Schema)
            //         .RequireAuthenticatedUser()
            //         .Build();
            // });
        }
        
        public static void AddJwt(this WebApplicationBuilder builder)
        {
            
            byte[] key = Encoding.UTF8.GetBytes(builder.Configuration["Authentication:Jwt:SecretKey"]);

            builder.Services.AddAuthentication(options =>
                {
                    // options.DefaultAuthenticateScheme = AuthSchema.Schema;
                    // options.DefaultChallengeScheme = AuthSchema.Schema;
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(AuthSchema.Schema, options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
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
                    var tokenService = context.RequestServices.GetRequiredService<ITokenRepository>();
                    var token = context.Request.Headers["Authorization"].ToString().Split(' ').Last();
                    
                    if (!string.IsNullOrEmpty(token) && 
                        !await tokenService.IsTokenValidAsync(token))
                    {
                        var tokenFactory = context.RequestServices.GetRequiredService<IJwtTokenFactory>();

                        if (tokenService.IsTokenExpired(token) && tokenFactory.IsTokenRefreshable(token))
                        {
                            var newToken = tokenFactory.GenerateNewJwtToken(context.User);
                            context.Response.Headers.Add("Authorization", $"{AuthSchema.Schema} {newToken}");
                        }
                        else
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync("Internal Server Error");
                    return;
                }

                await next();
            });
        }

        #endregion

        #region Cors

        public static void AddCors(this WebApplicationBuilder builder)
        {
            string origin = builder.Configuration.GetValue<string>("Origin") ?? string.Empty;

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins",
                    builder =>
                    {
                        builder.WithOrigins(origin.Split(","))
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials(); // If cookies or authorization headers are needed
                    });
            });
        }

        #endregion

        #region Databases

        public static void AddDatabaseContext<TDataContext>(this WebApplicationBuilder builder,
            string dbName = "Database")
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

            // builder.Services.AddScoped<ITokenRepository, DynamoDbTokenRepository>();
            //TODO replace to DynamoDB

            var redisConnectionString = builder.Configuration.GetSection("Redis")["ConnectionString"];
            builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));

            builder.Services.AddScoped<ITokenRepository, RedisTokenRepository>();
            builder.Services.AddScoped<ILocalizationRepository, RedisLocalizationRepository>();
        }

        #endregion

        #region Swagger

        public static void AddSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddOpenApiDocument();

            builder.Services.AddSwaggerDocument(config => { config.DocumentName = "swagger"; });

            string version = builder.Configuration.GetVersion();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(version,
                    new OpenApiInfo { Title = builder.Configuration["Microservice:Title"], Version = version });
                c.AddSecurityDefinition(AuthSchema.Schema, new OpenApiSecurityScheme
                {
                    Description = @$"JWT Authorization header using the {AuthSchema.Schema} scheme. \r\n\r\n 
                          Enter '{AuthSchema.Schema}' [space] and then your token in the text input below.
                          \r\n\r\nExample: '{AuthSchema.Schema} 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = AuthSchema.Schema
                });

                c.OperationFilter<CustomOperationIdFilter>();

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = AuthSchema.Schema
                            },
                            Scheme = "oauth2",
                            Name = AuthSchema.Schema,
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

        public static void AddSwagger(this IApplicationBuilder app, WebApplicationBuilder builder)
        {
            string version = builder.Configuration.GetVersion();
            
            app.UseDeveloperExceptionPage();
            app.UseSwagger(options => options.SerializeAsV2 = true);
            app.UseSwaggerUI(c =>
                c.SwaggerEndpoint(
                    $"/swagger/{version}/swagger.json",
                    $"{builder.Configuration["Microservice:Title"]} v{version}"));
        }
        
        private static string GetVersion(this IConfiguration configuration)
        {
            return $"{configuration["Microservice:Version"]}-{VersionGenerator.GetVersion()}";
        }

        #endregion
    }
}