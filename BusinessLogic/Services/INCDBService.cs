using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface INCDBService
    {
        Task<int> GetCrimeIndex(string ssn); 
    }
}
