using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class SSNFormatAttribute : RegularExpressionAttribute
    {
        public SSNFormatAttribute(): base(@"^\d{3}-\d{2}-\d{4}$")
        {
            ErrorMessage = "SSN must be in the format '000-00-0000'";
        }
    }

    public class Borrower
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "SSN is required")]
        [SSNFormat]
        public string SSN { get; set; }
        public string? MailingAddress { get; set; }
    }

}
