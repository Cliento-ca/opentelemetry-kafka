

public static class TelemetryConstants
{

    // Define some important constants and the activity source.
    // These can come from a config file, constants file, etc.
    public const string ServiceName = "MyCompany.MyProduct.MyService";
    public const string ServiceVersion = "1.0.0";
    public const string METRIC_PREFIX = "custom.metric.";

    public const string NUMBER_OF_EXEC_NAME = METRIC_PREFIX + "number.of.exec";
    public const string NUMBER_OF_EXEC_DESCRIPTION = "Count the number of executions.";

    public const string HEAP_MEMORY_NAME = METRIC_PREFIX + "heap.memory";
    public const string HEAP_MEMORY_DESCRIPTION = "Reports heap memory utilization.";

}
