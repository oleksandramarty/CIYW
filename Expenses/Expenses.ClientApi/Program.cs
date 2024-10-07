using AuthGateway.Mediatr;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CommonModule.Core.Strategies.GetFilteredResult;
using CommonModule.Facade;
using CommonModule.Shared.Responses.Expenses.Models.Expenses;
using Expenses.Business;
using Expenses.Domain;
using Expenses.GraphQL;
using Expenses.Mediatr;
using Expenses.Mediatr.Mediatr.Expenses.Requests;
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

// validators
builder.Services.AddValidatorsFromAssemblyContaining<CreateOrUpdateExpenseCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserProjectCommandValidator>();
builder.AddJwtAuthentication();
builder.AddDependencyInjection();

//GraphQL
builder.Services.AddSingleton<ISchema, ExpensesGraphQLSchema>(services => new ExpensesGraphQLSchema(new SelfActivatingServiceProvider(services)));
builder.AddGraphQL();

builder.Services.AddScoped<IBalanceRepository, BalanceRepository>();
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new MappingExpensesProfile());
});

// Strategies
builder.Services.AddScoped<IGetFilteredResultStrategy<GetFilteredExpensesRequest, ExpenseResponse>, GetFilteredResultOfExpenseStrategy>();
builder.Services.AddScoped<IGetFilteredResultStrategy<GetFilteredPlannedExpensesRequest, PlannedExpenseResponse>, GetFilteredResultOfPlannedExpenseStrategy>();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Host.ConfigureContainer<ContainerBuilder>(opts => { opts.RegisterModule(new MediatrExpensesModule()); });
builder.Host.ConfigureContainer<ContainerBuilder>(opts => { opts.RegisterModule(new MediatrCommonModule()); });

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