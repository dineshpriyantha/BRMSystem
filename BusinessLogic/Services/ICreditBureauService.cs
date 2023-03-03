using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface ICreditBureauService
    {
        Task<int> GetCreditScore(string ssn);
    }
}
