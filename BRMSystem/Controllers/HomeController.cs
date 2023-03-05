using BRMSystem.EventGenerator;
using BRMSystem.Models;
using BusinessLogic.Services;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace BRMSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBorrowerManager _borrower;
        private readonly IHubContext<EventHub> _hubContext;

        public HomeController(ILogger<HomeController> logger, 
                              IBorrowerManager borrower,
                              IHubContext<EventHub> hubContext)
        {
            _logger = logger;
            _borrower = borrower;
            _hubContext = hubContext;
        }

        public async Task GenerateEvents()
        {
            while (true)
            {
                // Generate event data
                var eventData = new Event
                {
                    Timestamp = DateTime.UtcNow,
                    Value = new Random().Next(1, 100)
                };

                // Send event data to SignalR hub
                await _hubContext.Clients.All.SendAsync("ReceiveEvent", eventData);

                // Wait for a specified interval before generating the next event
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }


        public IActionResult Index()
        {
            try
            {
                //GenerateEvents();
                EventGenerateHub generator1 = new EventGenerateHub(_hubContext);               


                var borrower = _borrower.GetBorrowers().Result;
                ViewBag.Borrowers = borrower;                
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Borrower borrower)
        {
            if(!ModelState.IsValid)
            {
                return View(borrower);         
            }

            try
            {
                _borrower.AddBorrower(borrower);
                ViewBag.message = "Data Saved Successfully..";
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            ModelState.Clear();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}