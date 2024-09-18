using IndividualsRegistry.Api.Configuration;
using IndividualsRegistry.Application;
using IndividualsRegistry.Persistence;
using IndividualsRegistry.Shared.Middleware;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host
    .UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration))
    .ConfigureAppConfiguration((_, configurationBuilder) => configurationBuilder.AddEnvironmentVariables());

builder.Services.AddControllers();
builder.Services.AddSettings(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly

builder.Services.AddHeaderPropagation();

builder.Services.AddResponseCompression(options => { options.EnableForHttps = true; });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "IndividualsRegistry.Api API V1", Version = "v1" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

app.UseMiddleware(typeof(ErrorHandlingMiddleware));

app.UseAuthorization();
app.UseResponseCompression();
app.MapControllers();
Log.Information("Finished Configuration");

app.Run();
