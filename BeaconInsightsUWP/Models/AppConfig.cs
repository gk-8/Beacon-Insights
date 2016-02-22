namespace BeaconInsightsUWP.Models
{
    public static class AppConfig
    {
        public static readonly double ZeroDistance = 0.0;
        public static readonly double ImmediateDistance = 1.0;
        public static readonly double NearDistance = 5.0;

        public static readonly string EventHubConnectionString = "Endpoint=sb://eventhubsensors-ns.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=OhnUbmXG4F1hjjXrUlk0McZzbA/oXyNgpYiKIZWHor4=";
        public static readonly string EventHubName = "dotnetspain-eh";
    }
}
