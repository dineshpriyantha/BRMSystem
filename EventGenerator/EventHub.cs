using DataAccessLayer.Models;
using Microsoft.AspNetCore.SignalR;

namespace BRMSystem.EventGenerator
{
    public class EventHub : Hub
    {
        public async Task SendEvent(Alert alertData)
        {
            // Send the event data to all connected clients
            await Clients.All.SendAsync("RecieveEvent", alertData);
        }
    }
}
