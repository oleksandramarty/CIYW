using AuthGateway.Mediatr;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CommonModule.Facade;
using Dictionaries.Domain;
using Dictionaries.GraphQL;
using Dictionaries.Mediatr;
using GraphQL.MicrosoftDI;
using GraphQL.Types;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.AddDatabaseContext<DictionariesDataContext>();
builder.AddDynamoDb();
builder.AddSwagger();
builder.AddCorsPolicy();
builder.Services.AddControllers();
builder.AddAuthorization();

builder.AddJwtAuthentication();
builder.AddDependencyInjection();

// Fluent validation starts
// Fluent validation ends

//GraphQL
builder.Services.AddSingleton<ISchema, DictionariesGraphQLSchema>(services => new DictionariesGraphQLSchema(new SelfActivatingServiceProvider(services)));
// GraphQL schema ends

builder.AddGraphQl();

// Custom DI
// Custom DI ends

// AutoMapper
builder.Services.AddAutoMapper(config => { config.AddProfile(new MappingDictionariesProfile()); });
// AutoMapper ends

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// MediatR modules
builder.Host.ConfigureContainer<ContainerBuilder>(opts => { opts.RegisterModule(new MediatrDictionariesModule()); });
builder.Host.ConfigureContainer<ContainerBuilder>(opts => { opts.RegisterModule(new MediatrCommonModule()); });
// MediatR modules ends

// Strategies
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