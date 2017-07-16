using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA.PracticeService.EF_Bulk_Practice.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
