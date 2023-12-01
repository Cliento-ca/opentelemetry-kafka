using System.Diagnostics;
using System.Diagnostics.Metrics;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var resource = ResourceBuilder.CreateDefault().AddService(TelemetryConstants.ServiceName, serviceVersion: TelemetryConstants.ServiceVersion);

var meter = new Meter(TelemetryConstants.ServiceName, TelemetryConstants.ServiceVersion);
var activitySource = new ActivitySource(TelemetryConstants.ServiceName, TelemetryConstants.ServiceVersion);

static void ConfigureOtlpExporter(OtlpExporterOptions options)
{
  options.Protocol = OtlpExportProtocol.Grpc;
  options.Endpoint = new Uri("http://host.docker.internal:5555");
}

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton(meter);
builder.Services.AddSingleton(activitySource);

builder.Services.AddOpenTelemetry()
  .WithMetrics(b => b
    .SetResourceBuilder(resource)
    .AddMeter(meter.Name)
    // .AddAspNetCoreInstrumentation()
    .AddOtlpExporter(options => ConfigureOtlpExporter(options))
    .AddConsoleExporter()
    )
  .WithTracing(b => b
    .AddSource(activitySource.Name)
    .SetResourceBuilder(resource)
    // .AddAspNetCoreInstrumentation()
    .AddOtlpExporter(options => ConfigureOtlpExporter(options))
    .AddConsoleExporter()
  );



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
