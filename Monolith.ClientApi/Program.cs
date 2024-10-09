using AuthGateway.GraphQL;
using AuthGateway.Mediatr;
using AuthGateway.Mediatr.Validators.Auth;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CommonModule.Core.Strategies.GetFilteredResult;
using CommonModule.Facade;
using CommonModule.Shared.Responses.Expenses.Models.Expenses;
using CommonModule.Shared.Responses.Expenses.Models.Projects;
using Dictionaries.GraphQL;
using Dictionaries.Mediatr;
using Expenses.Business;
using Expenses.Domain;
using Expenses.GraphQL;
using Expenses.Mediatr;
using Expenses.Mediatr.Mediatr.Expenses.Requests;
using Expenses.Mediatr.Mediatr.Projects.Requests;
using Expenses.Mediatr.Strategies.GetFilteredResult;
using Expenses.Mediatr.Validators.Expenses;
using Expenses.Mediatr.Validators.Projects;
using FluentValidation;
using GraphQL.MicrosoftDI;
using GraphQL.Types;
using Localizations.GraphQL;
using Localizations.Mediatr;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.AddDatabaseContext<ExpensesDataContext>();
builder.AddDynamoDB();
builder.AddSwagger();
builder.AddCorsPolicy();
builder.Services.AddControllers();
builder.AddAuthorization();

builder.AddJwtAuthentication();
builder.AddDependencyInjection();

// Fluent validation starts
builder.Services.AddValidatorsFromAssemblyContaining<CreateExpenseCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateExpenseCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreatePlannedExpenseCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdatePlannedExpenseCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserProjectCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<AuthSignUpCommandValidator>();
// Fluent validation ends

// GraphQL schema
builder.Services.AddSingleton<ISchema, ExpensesGraphQLSchema>(services => new ExpensesGraphQLSchema(new SelfActivatingServiceProvider(services)));
builder.Services.AddSingleton<ISchema, LocalizationsGraphQLSchema>(services => new LocalizationsGraphQLSchema(new SelfActivatingServiceProvider(services)));
builder.Services.AddSingleton<ISchema, DictionariesGraphQLSchema>(services => new DictionariesGraphQLSchema(new SelfActivatingServiceProvider(services)));
builder.Services.AddSingleton<ISchema, AuthGatewayGraphQLSchema>(services => new AuthGatewayGraphQLSchema(new SelfActivatingServiceProvider(services)));
// GraphQL schema ends

builder.AddGraphQL();

// Custom DI
builder.Services.AddScoped<IBalanceRepository, BalanceRepository>();
// Custom DI ends

// AutoMapper
builder.Services.AddAutoMapper(config => { config.AddProfile(new MappingExpensesProfile()); });
builder.Services.AddAutoMapper(config => { config.AddProfile(new MappingLocalizationsProfile()); });
builder.Services.AddAutoMapper(config => { config.AddProfile(new MappingDictionariesProfile()); });
builder.Services.AddAutoMapper(config => { config.AddProfile(new MappingAuthProfile()); });
// AutoMapper ends

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// MediatR modules
builder.Host.ConfigureContainer<ContainerBuilder>(opts => { opts.RegisterModule(new MediatrExpensesModule()); });
builder.Host.ConfigureContainer<ContainerBuilder>(opts => { opts.RegisterModule(new MediatorLocalizationsModule()); });
builder.Host.ConfigureContainer<ContainerBuilder>(opts => { opts.RegisterModule(new MediatrDictionariesModule()); });
builder.Host.ConfigureContainer<ContainerBuilder>(opts => { opts.RegisterModule(new MediatrAuthModule()); });
builder.Host.ConfigureContainer<ContainerBuilder>(opts => { opts.RegisterModule(new MediatrCommonModule()); });
// MediatR modules ends

// Strategies
builder.Services.AddScoped<IGetFilteredResultStrategy<GetFilteredExpensesRequest, ExpenseResponse>, GetFilteredResultOfExpenseStrategy>();
builder.Services.AddScoped<IGetFilteredResultStrategy<GetFilteredPlannedExpensesRequest, PlannedExpenseResponse>, GetFilteredResultOfPlannedExpenseStrategy>();
builder.Services.AddScoped<IGetFilteredResultStrategy<GetFilteredUserProjectsRequest, UserProjectResponse>, GetFilteredResultOfUserProjectStrategy>();
builder.Services.AddScoped<IGetFilteredResultStrategy<GetFilteredUserAllowedProjectsRequest, UserAllowedProjectResponse>, GetFilteredResultOfUserAllowedProjectStrategy>();
// Strategies end

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI(builder);
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