using Autofac;
using Autofac.Extensions.DependencyInjection;
using CommonModule.Facade;
using Localizations.Business;
using Localizations.Domain;
using Localizations.Mediatr;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.AddGoogleAuthentication();

// Configure PostgreSQL with EF Core
builder.AddDatabaseContext<LocalizationsDataContext>();

builder.AddSwagger();

builder.AddCors();

//Validators here
//builder.Services.AddValidatorsFromAssemblyContaining<AuthSignUpCommandValidator>();

builder.AddJwt();

builder.AddDependencyInjection();

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new MappingLocalizationsProfile());
});

//Strategies here

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Host.ConfigureContainer<ContainerBuilder>(opts => { opts.RegisterModule(new MediatorLocalizationsModule()); });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.AddSwagger(builder);
}

app.UseCors("AllowSpecificOrigins");

app.UseStaticFiles();

app.UseRouting();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseTokenValidator();
app.UseAuthorization();

app.MapControllers();

app.Run();