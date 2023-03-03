using DataAccessLayer.Models;
using Microsoft.AspNetCore.SignalR;

namespace BRMSystem.EventGenerator
{
    public class EventHub : Hub
    {
        public async Task SendEvent(Event eventData)
        {
            // Send the event data to all connected clients
            await Clients.All.SendAsync("RecieveEvent", eventData);
        }
    }
}
