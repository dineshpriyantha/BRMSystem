using BRMSystem.EventGenerator;
using BRMSystem.Models;
using BusinessLogic.Services;
using DataAccessLayer.Models;
using EventGenerator;
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

        public IActionResult Index()
        {
            try
            {
                AlertManager alert = new(_borrower, _hubContext);
                alert.ProcessAlerts();

                var borrower = _borrower.GetBorrowers().Result;
                ViewBag.Borrowers = borrower;                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured in the Index action.");
                return Error(ex);
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
                _logger.LogError(ex, "An error occured in the Create action");
                return Error(ex);
            }

            ModelState.Clear();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(Exception ex)
        {
            var errorViewModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            _logger.LogError(ex, "An unhandled exception occurred.");

            return View(errorViewModel);
        }
    }
}