using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TA.PracticeService.EF_Bulk_Practice.Models;

namespace TA.PracticeService.EF_Bulk_Practice
{
    public static class EfBulkPractice
    {
        public static void Run()
        {
            using (var context = new EmployeeContext())
            {
                Console.Clear();
                //var employees = context.Employees.Where(w => w.Forename == "Abhinav");
                //context.Employees.RemoveRange(employees);
                //var employee = new Employee {
                //    Id = 1,
                //    Forename = "Abhinav",
                //    Surname = "Pisharody",
                //    JoiningDate = DateTime.Now,
                //    MiddleName = "Jairaj",
                //    PayrollId = "1",
                //    Department = new Department
                //    {
                //        Id = 1,
                //        Name = "Software",
                //        Description = "IT Field Department"
                //    }
                //};
                const int records = 1000000;
                var employees = new List<Employee>();
                for (var index = 0; index < records; index++)
                {
                    var employee = new Employee
                    {
                        Id = index+1,
                        Forename = $"TestFirstName{index+1}",
                        Surname = $"TestSecondName{index+1}",
                        JoiningDate = DateTime.Now,
                        MiddleName = $"TestMiddleName{index+1}",
                        PayrollId = $"{index+1}"
                    };
                    employees.Add(employee);
                }
                var insertStopwatch = new Stopwatch();
                insertStopwatch.Start();
                context.BulkInsert(employees);
                insertStopwatch.Stop();
                Console.WriteLine($"Total Time Taken To Insert {records} records: {insertStopwatch.ElapsedMilliseconds}");

                //var employeesToUpdate = new List<Employee>();
                //for (var index = 0; index < records; index++)
                //{
                //    var employee = new Employee
                //    {
                //        Id = index + 1,
                //        Forename = $"UpdatedTestFirstName{index + 1}",
                //        Surname = $"UpdatedTestSecondName{index + 1}",
                //        JoiningDate = DateTime.Now,
                //        MiddleName = $"UpdatedTestMiddleName{index + 1}",
                //        PayrollId = $"{index + 1}"
                //    };
                //    employeesToUpdate.Add(employee);
                //}
                var updateStopwatch = new Stopwatch();
                updateStopwatch.Start();
                context.Employees.UpdateFromQuery(emp => new Employee
                {
                    Forename = $"UpdatedTestFirstName",
                    Surname = $"UpdatedTestSecondName",
                    JoiningDate = DateTime.Now,
                    MiddleName = $"UpdatedTestMiddleName",
                    PayrollId = $"1"
                }, op => op.BatchTimeout = 60);
                //context.BulkUpdate(employeesToUpdate);
                updateStopwatch.Stop();
                Console.WriteLine($"Total Time Taken To Update {records} records: {updateStopwatch.ElapsedMilliseconds}");

                //var deleteEmployees = new List<Employee>();
                //for (var index = 0; index < records; index++)
                //{
                //    var employee = new Employee
                //    {
                //        Id = index + 1,
                //        Forename = $"TestFirstName{index + 1}",
                //        Surname = $"TestSecondName{index + 1}",
                //        JoiningDate = DateTime.Now,
                //        MiddleName = $"TestMiddleName{index + 1}",
                //        PayrollId = $"{index + 1}"
                //    };
                //    deleteEmployees.Add(employee);
                //}

                var deleteStopwatch = new Stopwatch();
                deleteStopwatch.Start();
                context.Employees.DeleteFromQuery(op => op.BatchTimeout = 60);
                //context.BulkDelete(deleteEmployees);
                deleteStopwatch.Stop();
                Console.WriteLine($"Total Time Taken To Delete {records} records: {deleteStopwatch.ElapsedMilliseconds}");

                //var stopwatch = new Stopwatch();
                //stopwatch.Start();
                //context.SaveChanges();
                //context.BulkSaveChanges();
                //stopwatch.Stop();
                //Console.WriteLine($"Total Time Taken To Save {records} records: {stopwatch.ElapsedMilliseconds}");
                Console.ReadKey();
            }
        }
    }
}
