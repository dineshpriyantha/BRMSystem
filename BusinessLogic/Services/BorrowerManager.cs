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

        public async Task<bool> AddBorrower(Borrower borrower)
        {
            if(borrower == null)
            {
                return false;
            }

            // Check if a borrower with the same name or SSN already exists in the database
            var existingBorrower = await _context.Borrowers.FirstOrDefaultAsync(b =>
                b.Name == borrower.Name || b.SSN == borrower.SSN);

            if (existingBorrower != null)
            {
                return false;
            }

            _context.Borrowers.Add(borrower);
            var rowsAffected = await _context.SaveChangesAsync();

            return rowsAffected > 0;
        }

        public async Task<bool> UpdateBorrower(Borrower borrower)
        {
            if (borrower == null)
            {
                return false;
            }

            var existingBorrower = await _context.Borrowers.FirstOrDefaultAsync(s => s.Id == borrower.Id);

            if (existingBorrower == null)
            {
                return false;
            }

            existingBorrower.Name = borrower.Name;
            existingBorrower.DateOfBirth = borrower.DateOfBirth;
            existingBorrower.SSN = borrower.SSN;
            existingBorrower.MailingAddress = borrower.MailingAddress;

            _context.Borrowers.Update(existingBorrower);
            var rowsAffected = await _context.SaveChangesAsync();

            return rowsAffected > 0;
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