namespace BeaconInsightsUWP.Services.Interfaces
{
    public interface IAzureEventHubService
    {
        void SendMessage(string message);
    }
}
