using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Alert
    {
        public int Id { get; set; }
        public int BorrowerId { get; set; }
        public AlertType Type { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
    }

    public enum AlertType
    {
        CriminalRecord,
        LowCreditScore
    }
}
