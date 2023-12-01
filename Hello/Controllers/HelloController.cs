using System.Diagnostics;
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Mvc;
namespace Hello.Controllers;

[ApiController]
[Route("[controller]")]
public class HelloController : ControllerBase
{
    private readonly ILogger<HelloController> _logger;
    private readonly ActivitySource _activitySource;
    private readonly Counter<long> _successCounter;
    public HelloController(ILogger<HelloController> logger, Meter meter, ActivitySource activitySource)
    {
        _logger = logger;
        _activitySource = activitySource;

        _successCounter = meter.CreateCounter<long>(TelemetryConstants.NUMBER_OF_EXEC_NAME, description: TelemetryConstants.NUMBER_OF_EXEC_DESCRIPTION);

        meter.CreateObservableGauge(TelemetryConstants.HEAP_MEMORY_NAME, () =>
           {
               long totalMemory = GC.GetTotalMemory(false);
               long freeMemory = GC.GetTotalMemory(true);
               return (byte)(totalMemory - freeMemory);
           },
           description: TelemetryConstants.HEAP_MEMORY_DESCRIPTION);
    }


    [HttpGet(Name = "Hello")]
    public string Get()
    {
        using var activity = _activitySource.StartActivity("SayHello");
        {
            activity?.SetTag("foo", 1);
            activity?.SetTag("bar", "Hello, World!");
            activity?.SetTag("baz", new int[] { 1, 2, 3 });

            activity?.SetStatus(ActivityStatusCode.Ok);

            _successCounter.Add(1);
        }

        return "Hello World!";

    }
}
