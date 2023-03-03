using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BRMSystem.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditBureauController : ControllerBase
    {
        private static readonly Dictionary<string, int> _creditScores = new Dictionary<string, int>()
        {
            {"111-11-1111", 750 },
            {"222-22-2222", 600 },
            {"333-33-3333", 700 },
            {"444-44-4444", 900 }
        };

        [HttpGet]
        public ActionResult<Dictionary<string, int>> GetCreditScore()
        {
            return Ok(_creditScores);
        }

        [HttpGet("{ssn}")]
        public ActionResult<int> GetCreditScore(string ssn)
        {
            if (_creditScores.ContainsKey(ssn))
            {
                return Ok(_creditScores[ssn]);
            }

            return NotFound();
        }
    }
}
