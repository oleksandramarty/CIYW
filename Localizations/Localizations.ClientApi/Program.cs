using Autofac;
using Autofac.Extensions.DependencyInjection;
using CommonModule.Facade;
using FluentValidation;
using Localizations.Business;
using Localizations.Domain;
using Localizations.Mediatr;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.AddDatabaseContext<LocalizationsDataContext>();
builder.AddDynamoDB();
builder.AddSwagger();
builder.AddCors();
builder.Services.AddControllers();
builder.AddAuthorization();

// validators
builder.AddJwt();
builder.AddDependencyInjection();
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new MappingLocalizationsProfile());
});
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
app.UseAuthorization();
app.UseTokenValidator();
app.MapControllers();
app.Run();