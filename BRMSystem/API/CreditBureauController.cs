﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BRMSystem.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditBureauController : ControllerBase
    {
        private static readonly Dictionary<string, int> _creditScores = new Dictionary<string, int>()
        {
            {"111-11-1111", 50 },
            {"222-22-2222", 600 },
            {"333-33-3333", 500 },
            {"444-44-4444", 900 },
            {"555-55-5555", 300 },
            {"666-66-6666", 400 },
            {"777-77-7777", 800 },
            {"888-88-8888", 680 },
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
