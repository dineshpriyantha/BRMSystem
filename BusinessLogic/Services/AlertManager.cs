using BRMSystem.EventGenerator;
using DataAccessLayer;
using DataAccessLayer.Models;
using EventGenerator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
    public class AlertManager : INCDBService, ICreditBureauService
    {
        private readonly IBorrowerManager _borrower;
        private readonly IHubContext<EventHub> _hubContext;
        private readonly IConfiguration _configuration;

        public AlertManager(IBorrowerManager borrower, 
                            IHubContext<EventHub> hubContext,
                            IConfiguration configuration)
        {
            _borrower = borrower;
            _configuration = configuration;
            _hubContext = hubContext;
        }

        public async void ProcessAlerts()
        {
            var borrowes = _borrower.GetBorrowers().Result;

            foreach (var borrower in borrowes)
            {
                int ncdbIndex = await GetCrimeIndex(borrower.SSN);
                int creditScore = await GetCreditScore(borrower.SSN);

                if (ncdbIndex > 0)
                {
                    Alert alert = new Alert()
                    {
                        Type = AlertType.CriminalRecord,
                        BorrowerId = borrower.Id,
                        Date = DateTime.Now,
                        Message = $"The borrower name: {borrower.Name} <br/> SSN : {borrower.SSN} <br/> Crime Index : {ncdbIndex}"
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
                        Message = $"The borrower name: {borrower.Name} <br/> SSN : {borrower.SSN} <br/> Credit Score: {creditScore}"
                    };
                    EventGenerateHub eventHub = new EventGenerateHub(_hubContext, alert);
                }
            }
        }

        public HttpClient GetHttpClient()
        {
            string? apiUrl = _configuration.GetSection("AppSettings")["ApiUrl"];
            if (apiUrl != null)
            {
                var client = new HttpClient()
                {
                    BaseAddress = new Uri(apiUrl)
                };
                return client;
            }
            else
            {
                throw new Exception("Apiurl is not configured");
            }
        }

        public async Task<int> GetCreditScore(string borrowerSSN)
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

        public async Task<int> GetCrimeIndex(string borrowerSSN)
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
    }

}
