using BRMSystem.EventGenerator;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.SignalR;

namespace EventGenerator
{
    public class EventGenerateHub
    {
        private readonly IHubContext<EventHub> _hubContext;
        private readonly Timer _timer;

        public EventGenerateHub(IHubContext<EventHub> hubContext)
        {
            _hubContext = hubContext;
            _timer = new Timer(GenerateEvents, null, 0, 5000); // Generate events every 5 seconds
        }

        private void GenerateEvents(object state)
        {
            var eventData = new Event
            {
                Timestamp = DateTime.UtcNow,
                Value = new Random().Next(1, 100)
            };

            // Send event to clients
            _hubContext.Clients.All.SendAsync("ReceiveEvent", eventData);
        }
    }
}