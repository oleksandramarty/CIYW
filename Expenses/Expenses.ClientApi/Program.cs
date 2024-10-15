using AuthGateway.Mediatr;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CommonModule.Core.Strategies.GetFilteredResult;
using CommonModule.Facade;
using CommonModule.Shared.Responses.Expenses.Models.Expenses;
using CommonModule.Shared.Responses.Expenses.Models.Projects;
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
builder.Services.AddValidatorsFromAssemblyContaining<CreateFavoriteExpenseCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateFavoriteExpenseCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateExpenseCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateExpenseCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreatePlannedExpenseCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdatePlannedExpenseCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserProjectCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateUserProjectCommandValidator>();
// Fluent validation ends

// GraphQL schema
builder.Services.AddSingleton<ISchema, ExpensesGraphQLSchema>(services => new ExpensesGraphQLSchema(new SelfActivatingServiceProvider(services)));
// GraphQL schema ends

builder.AddGraphQL();

// Custom DI
builder.Services.AddScoped<IBalanceRepository, BalanceRepository>();
// Custom DI ends

// AutoMapper
builder.Services.AddAutoMapper(config => { config.AddProfile(new MappingExpensesProfile()); });
// AutoMapper ends

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// MediatR modules
builder.Host.ConfigureContainer<ContainerBuilder>(opts => { opts.RegisterModule(new MediatrExpensesModule()); });
builder.Host.ConfigureContainer<ContainerBuilder>(opts => { opts.RegisterModule(new MediatrCommonModule()); });
// MediatR modules ends

// Strategies
builder.Services.AddScoped<IGetFilteredResultStrategy<GetFilteredExpensesRequest, ExpenseResponse>, GetFilteredResultOfExpenseStrategy>();
builder.Services.AddScoped<IGetFilteredResultStrategy<GetFilteredPlannedExpensesRequest, PlannedExpenseResponse>, GetFilteredResultOfPlannedExpenseStrategy>();
builder.Services.AddScoped<IGetFilteredResultStrategy<GetFilteredFavoriteExpensesRequest, FavoriteExpenseResponse>, GetFilteredResultOfFavoriteExpenseStrategy>();
builder.Services.AddScoped<IGetFilteredResultStrategy<GetFilteredUserProjectsRequest, UserProjectResponse>, GetFilteredResultOfUserProjectStrategy>();
builder.Services.AddScoped<IGetFilteredResultStrategy<GetFilteredUserAllowedProjectsRequest, UserAllowedProjectResponse>, GetFilteredResultOfUserAllowedProjectStrategy>();
// Strategies end

var app = builder.Build();

app.AddMiddlewares();

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