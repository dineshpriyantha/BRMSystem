using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NCDBService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NCDBController : ControllerBase
    {
        [HttpGet("{ssn}")]
        public ActionResult<int> GetCrimeIndex(string ssn)
        {
            // This is a simulated crime index based on the last digit of the SSN
            int lastDigit = int.Parse(ssn.Substring(ssn.Length - 1));
            int crimeIndex = lastDigit * 10;

            return Ok(crimeIndex);
        }
    }
}
