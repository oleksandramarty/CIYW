using AuditTrail.Business;
using AuditTrail.Domain;
using CommonModule.Facade;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.Services.AddDbContext<AuditTrailDataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddHostedService<KafkaConsumer>();

var app = builder.Build();

app.AddMiddlewares();

app.Run();