using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA.PracticeService.EF_Bulk_Practice.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public DateTime? JoiningDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string MiddleName { get; set; }
        public string PayrollId { get; set; }
        public Department Department { get; set; }
    }
}
