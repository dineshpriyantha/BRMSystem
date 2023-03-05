using BRMSystem.EventGenerator;
using DataAccessLayer;
using DataAccessLayer.Models;
using EventGenerator;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class AlertManager
    {
        private readonly IBorrowerManager _borrower;
        private readonly IHubContext<EventHub> _hubContext;
        //private readonly Timer _timer;

        public AlertManager(IBorrowerManager borrower, IHubContext<EventHub> hubContext)
        {
            _borrower = borrower;
            _hubContext = hubContext;
        }

        public async void ProcessAlerts()
        {
            try
            {
                var borrowes = _borrower.GetBorrowers().Result;                

                foreach (var borrower in borrowes)
                {
                    int ncdbIndex = await GetDataFromNCBD(borrower.SSN);
                    int creditScore = await GetDataFromCreditBureau(borrower.SSN);

                    if (ncdbIndex > 0)
                    {
                        Alert alert = new Alert()
                        {
                            Type = AlertType.CriminalRecord,
                            BorrowerId = borrower.Id,
                            Date = DateTime.Now,
                            Message = $"The borrower name: {borrower.Name} SSN : {borrower.SSN} crime Index : {ncdbIndex}"
                        };
                        EventGenerateHub eventHub = new EventGenerateHub(_hubContext, alert);
                    }

                    if (0 < creditScore && creditScore < 600)
                    {
                        Alert alert = new Alert()
                        {
                            Type = AlertType.LowCreditScore,
                            BorrowerId = borrower.Id,
                            Date = DateTime.Now,
                            Message = $"The borrower name: {borrower.Name} SSN : {borrower.SSN} Credit Score: {creditScore}"
                        };
                        EventGenerateHub eventHub = new EventGenerateHub(_hubContext, alert);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private HttpClient GetHttpClient()
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri("https://localhost:7034/api/")
            };
            return client;
        }

        private async Task<int> GetDataFromCreditBureau(string borrowerSSN)
        {
            using (var client = GetHttpClient())
            {                
                try
                {
                    HttpResponseMessage response = await client.GetAsync($"creditbureau/{borrowerSSN}");
                    response.EnsureSuccessStatusCode();
                    string responseString = await response.Content.ReadAsStringAsync();
                    if (int.TryParse(responseString, out int creditScore))
                    {
                        return creditScore;
                    }
                    else
                    {
                        return -1;
                    }
                }
                catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return -1;
                }           
            }
        }

        private async Task<int> GetDataFromNCBD(string borrowerSSN)
        {
            using (var client = GetHttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync($"ncdb/{borrowerSSN}");
                    response.EnsureSuccessStatusCode();
                    string responseString = await response.Content.ReadAsStringAsync();                    
                    if (int.TryParse(responseString, out int ncdbIndex))
                    {
                        return ncdbIndex;
                    }
                    else
                    {
                        return -1;
                    }
                }
                catch (HttpRequestException ex) when (ex.StatusCode== HttpStatusCode.NotFound)
                {
                    return -1;
                }
            }
        }

        // Other methods to add, retrieve, and delete alerts
    }

}
