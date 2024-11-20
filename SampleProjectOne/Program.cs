using Microsoft.AspNetCore.HttpLogging;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.RequestBody | HttpLoggingFields.ResponseBody | HttpLoggingFields.RequestQuery | HttpLoggingFields.Response;
});

var configuration = builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
        optional: true)
    .AddEnvironmentVariables()
    .Build();

Log.Logger = new LoggerConfiguration()
           .ReadFrom.Configuration(configuration)
           .CreateLogger();

builder.Services.AddHttpClient();

builder.Host.UseSerilog();

// Tracing setup
builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
    {
        tracerProviderBuilder
            .SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddService("SampleProjectOne")) // Esme service ro tarif kon
            .AddAspNetCoreInstrumentation()    // Baraye trace kardan request-ha
            .AddHttpClientInstrumentation()    // Baraye trace kardan HttpClient
            .AddJaegerExporter(opts =>         // Export traces to Jaeger
            {
                opts.AgentHost = "localhost";  // IP/Hostname of Jaeger server
                opts.AgentPort = 6831;         // Default Jaeger port
            });
    });

// Metrics setup
builder.Services.AddOpenTelemetry()
    .WithMetrics(metricsBuilder =>
    {
        metricsBuilder
            .SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddService("SampleProjectOneMetrics")) // Esme metrics service
            .AddAspNetCoreInstrumentation()    // Automatic HTTP metrics
            .AddHttpClientInstrumentation()    // Automatic HTTP Client metrics
            .AddMeter("CustomMetrics")         // Custom metric source
            .AddPrometheusExporter();          // Export metrics to Prometheus
    });


var app = builder.Build();

app.MapPrometheusScrapingEndpoint();

app.UseHttpLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
