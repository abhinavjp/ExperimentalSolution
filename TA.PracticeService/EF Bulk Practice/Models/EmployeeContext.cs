using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA.PracticeService.EF_Bulk_Practice.Models
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext()
            : base()
        {

        }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
    }
}
