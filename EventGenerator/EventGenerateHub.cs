using BRMSystem.EventGenerator;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.SignalR;

namespace EventGenerator
{
    public class EventGenerateHub
    {
        private readonly IHubContext<EventHub> _hubContext;
        private readonly Timer _timer;
        private readonly Alert _alert;

        public EventGenerateHub(IHubContext<EventHub> hubContext, Alert alert)
        {
            _hubContext = hubContext;
            _alert = alert;
            _timer = new Timer(GenerateEvents, null, 0, 5000); // Generate events every 5 seconds            
        }

        public void GenerateEvents(object? data)
        {
            // Send event to clients
            _hubContext.Clients.All.SendAsync("ReceiveEvent", _alert);
        }
    }
}