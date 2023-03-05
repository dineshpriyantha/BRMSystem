using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BRMSystem.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class NCDBController : ControllerBase
    {
        private static readonly Dictionary<string, int> _crimeIndexes = new Dictionary<string, int>()
        {
            {"111-11-1112", 80 },
            {"222-22-2222", 20 },
            {"333-33-3333", 50 },
            {"444-44-4443", 60 }
        };

        [HttpGet]
        public ActionResult<Dictionary<string, int>> GetCrimeIndex()
        {
            return Ok(_crimeIndexes);
        }

        [HttpGet("{ssn}")]
        public ActionResult<int> GetCrimeIndex(string ssn)
        {
            if (_crimeIndexes.ContainsKey(ssn))
            {
                return Ok(_crimeIndexes[ssn]);
            }

            return NotFound();
        }
    }
}
