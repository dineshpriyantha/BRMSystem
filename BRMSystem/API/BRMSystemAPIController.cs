using BusinessLogic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BRMSystem.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class BRMSystemAPIController : ControllerBase, IBRMSService
    {
        // Define a method to get a instance of HttpClient with a base address
        private HttpClient GetHttpClient()
        {
            var client = new HttpClient() 
            { 
                BaseAddress = new Uri("https://localhost:7034/api/") 
            };
            return client;
        }

        [HttpGet("creditbureau/{borrowerSSN}")]
        public async Task<ActionResult> ReceiveAlertFromCreditBureau(string borrowerSSN)
        {         
            using var client = GetHttpClient();
            try
            {
                // Send the Http Get request to the bureau API endpoint with the borrower's SSN
                HttpResponseMessage response = await client.GetAsync($"CreditBureau/{borrowerSSN}");

                // Check if the response indicates success
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return Ok(result);
                }
                // Check if the response indicates that the requested resource was not found (Http status code 404)
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return NotFound($"SSN :{borrowerSSN} not found");
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server error");
                }
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("ncdb/{borrowerSSN}")]
        public async Task<ActionResult> ReceiveAlertFromNCDB(string borrowerSSN)
        {
            using(var client = GetHttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync($"NCDB/{borrowerSSN}");

                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        return Ok(result);
                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        return NotFound($"SSN :{borrowerSSN} not found");
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, "Internam Server error");
                    }
                }
                catch (HttpRequestException ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }                
            }
        }
    }
}
