namespace BeaconInsightsUWP.Services.Interfaces
{
    public interface INotificationsService
    {
        void NotifyWithUrl(string title, string message, string url);
        void NotifyWithTemperature(string title, string message, float temperature);
        void NotifyWithDistance(string title, string message, double distance);
    }
}
