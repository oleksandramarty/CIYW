using AuditTrail.Business;
using AuditTrail.Domain;
using AuditTrail.GraphQL;
using AuditTrail.Mediatr;
using AuditTrail.Mediatr.Mediatr.Requests;
using AuditTrail.Mediatr.Strategies.FilteredResult;
using AuthGateway.Mediatr;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CommonModule.Core.Strategies.FilteredResult;
using CommonModule.Facade;
using CommonModule.Shared.Responses.AuditTrail;
using Expenses.Mediatr.Strategies.FilteredResult;
using GraphQL.MicrosoftDI;
using GraphQL.Types;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.AddDatabaseContext<AuditTrailDataContext>("Logs");
builder.AddDynamoDb();
builder.AddSwagger();
builder.AddCorsPolicy();
builder.Services.AddControllers();
builder.AddAuthorization();

builder.AddJwtAuthentication();
builder.AddDependencyInjection();

// Fluent validation starts
// Fluent validation ends

// GraphQL schema
builder.Services.AddSingleton<ISchema, AuditTrailGraphQLSchema>(services => new AuditTrailGraphQLSchema(new SelfActivatingServiceProvider(services)));
// GraphQL schema ends

builder.AddGraphQl();

// Custom DI
builder.Services.AddScoped<IAuditTrailRepository, AuditTrailRepository>();
// Custom DI ends

// AutoMapper
builder.Services.AddAutoMapper(config => { config.AddProfile(new MappingAuditTrailProfile()); });
// AutoMapper ends

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// MediatR modules
builder.Host.ConfigureContainer<ContainerBuilder>(opts => { opts.RegisterModule(new MediatrAuditTrailModule()); });
builder.Host.ConfigureContainer<ContainerBuilder>(opts => { opts.RegisterModule(new MediatrCommonModule()); });
// MediatR modules ends

// Strategies
builder.Services.AddScoped<IFilteredResultStrategy<FilteredAuditTrailRequest, AuditTrailResponse>, FilteredResultOfAuditTrailStrategy>();
// Strategies end

var app = builder.Build();

app.AddMiddlewares();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUi(builder);
    app.UseGraphQLPlayground("/graphql/playground");
}

app.UseCors("AllowSpecificOrigins");
app.UseStaticFiles();
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseTokenValidator();
app.MapControllers();
app.UseGraphQL();

app.Run();