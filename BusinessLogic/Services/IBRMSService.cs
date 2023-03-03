using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface IBRMSService
    {
        Task<ActionResult> ReceiveAlertFromNCDB(string borrowerSSN); //int crimeIndex
        Task<ActionResult> ReceiveAlertFromCreditBureau(string borrowerSSN); //double creditScore
    }
}
