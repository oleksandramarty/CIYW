using AuthGateway.Domain;
using AuthGateway.GraphQL;
using AuthGateway.Mediatr;
using AuthGateway.Mediatr.Validators.Auth;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CommonModule.Facade;
using FluentValidation;
using GraphQL.MicrosoftDI;
using GraphQL.Types;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.AddDatabaseContext<AuthGatewayDataContext>();
builder.AddDynamoDB();
builder.AddSwagger();
builder.AddCorsPolicy();
builder.Services.AddControllers();
builder.AddAuthorization();

// validators
builder.Services.AddValidatorsFromAssemblyContaining<AuthSignUpCommandValidator>();
builder.AddJwtAuthentication();
builder.AddDependencyInjection();

//GraphQL
builder.Services.AddSingleton<ISchema, AuthGatewayGraphQLSchema>(services => new AuthGatewayGraphQLSchema(new SelfActivatingServiceProvider(services)));
builder.AddGraphQL();

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new MappingAuthProfile());
});

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Host.ConfigureContainer<ContainerBuilder>(opts => { opts.RegisterModule(new MediatrAuthModule()); });
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