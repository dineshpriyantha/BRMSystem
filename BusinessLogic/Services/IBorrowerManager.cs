using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface IBorrowerManager
    {
        Task AddBorrower(Borrower borrower);
        Task UpdateBorrower(Borrower borrower);
        Task DeleteBorrower(int? borrowerId);
        Task<Borrower> GetBorrowerById(int? borrowerId);
        Task<IEnumerable<Borrower>> GetBorrowers();
    }
}
