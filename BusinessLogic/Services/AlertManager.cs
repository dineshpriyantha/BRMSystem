using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class AlertManager
    {
        private readonly BRMSContext _context;

        public AlertManager(BRMSContext context)
        {
            _context = context;
        }

        public void ProcessAlerts()
        {
            // Query for alerts and notify management as needed
        }

        // Other methods to add, retrieve, and delete alerts
    }

}
