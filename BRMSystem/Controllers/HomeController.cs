using BRMSystem.Models;
using BusinessLogic.Services;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BRMSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBorrowerManager _borrower;

        public HomeController(ILogger<HomeController> logger, IBorrowerManager borrower)
        {
            _logger = logger;
            _borrower = borrower;
        }

        public IActionResult Index()
        {
            try
            {
                var borrower = _borrower.GetBorrowers().Result;
                if(borrower != null)
                    ViewBag.Borrowers = borrower;                
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Borrower borrower)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    _borrower.AddBorrower(borrower);
                    ViewBag.message = "Data Saved Successfully..";
                }
                catch (Exception)
                {
                    throw;
                }                
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