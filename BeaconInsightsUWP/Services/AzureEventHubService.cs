using BeaconInsightsUWP.Models;
using BeaconInsightsUWP.Services.Interfaces;
using ppatierno.AzureSBLite;
using ppatierno.AzureSBLite.Messaging;
using System;
using System.Text;

namespace BeaconInsightsUWP.Services
{
    public class AzureEventHubService : IAzureEventHubService
    {
        private EventHubClient _client;

        public AzureEventHubService()
        {
            ServiceBusConnectionStringBuilder builder = new ServiceBusConnectionStringBuilder(AppConfig.EventHubConnectionString);
            builder.TransportType = TransportType.Amqp;

            MessagingFactory factory = MessagingFactory.CreateFromConnectionString(AppConfig.EventHubConnectionString);

            _client = factory.CreateEventHubClient(AppConfig.EventHubName);
        }

        public void SendMessage(string message)
        {
            EventData data = new EventData(Encoding.UTF8.GetBytes(message));
            data.Properties["time"] = DateTime.UtcNow;

            _client.Send(data);
        }
    }
}
