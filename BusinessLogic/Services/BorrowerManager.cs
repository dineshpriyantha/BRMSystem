using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class BorrowerManager : IBorrowerManager
    {
        private readonly BRMSContext _context;

        public BorrowerManager(BRMSContext context)
        {
            _context = context;
        }
        public async Task<Borrower> GetBorrowerById(int? borrowerId)
        {
            if(borrowerId == null)
            {
               throw new ArgumentNullException(nameof(borrowerId));
            }

            var borrower = await _context.Borrowers.FirstOrDefaultAsync(x => x.Id == borrowerId);
            if(borrower == null)
            {
                throw new ArgumentException("The specified borrower does not exist.", nameof(borrowerId));
            }

            return borrower;
        }

        public async Task<IEnumerable<Borrower>> GetBorrowers()
        {
            return await _context.Borrowers.ToListAsync();
        }

        public async Task AddBorrower(Borrower borrower)
        {
            if(borrower == null)
            {
                throw new ArgumentException(nameof(borrower));
            }

            _context.Borrowers.Add(borrower);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBorrower(Borrower borrower)
        {

            if (borrower == null)
            {
                throw new ArgumentNullException(nameof(borrower));
            }

            var existingBorrower = await _context.Borrowers.FirstOrDefaultAsync(s => s.Id == borrower.Id);

            if (existingBorrower == null)
            {
                throw new ArgumentException("The specified borrower does not exist.", nameof(borrower));
            }

            existingBorrower.Name = borrower.Name;
            existingBorrower.DateOfBirth = borrower.DateOfBirth;
            existingBorrower.SSN = borrower.SSN;
            existingBorrower.MailingAddress = borrower.MailingAddress;

            _context.Borrowers.Update(existingBorrower);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBorrower(int? borrowerId)
        {
            var borrower = await _context.Borrowers.FirstOrDefaultAsync(s => s.Id == borrowerId);

            if (borrower == null)
            {
                throw new ArgumentException("The specified borrower does not exist. ", nameof(borrowerId));
            }

            _context.Borrowers.Remove(borrower);
            await _context.SaveChangesAsync();
        }        
    }
}