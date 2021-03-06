﻿using RepositoryFoundation.Helper.ExpressionBuilder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using TA.PracticeService.CodeFirst.Entities;

namespace TA.PracticeService.EF_Bulk_Practice
{
    public static class EfBulkPractice
    {
        public static void Run()
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(180) }))
                {
                    try
                    {
                        using (var context = new EmployeeContext())
                        {
                            Console.Clear();
                            const int records = 10000;
                            InsertEmployee(context, records);
                            //UpdateEmployees(context, records);
                            //DeleteEmployees(context, records);
                        }


                        using (var context = new EmployeeContext())
                        {

                            Console.Clear();
                            const int records = 1000;
                            InsertDepartments(context, records);
                        }
                        scope.Complete();
                    }
                    catch (TransactionAbortedException)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write("Oh No!!!! Error while disposing transaction");
                    }
                }

                var employees = new List<TestMe>();
                for (var index = 0; index < 1000; index++)
                {
                    var employee = new TestMe
                    {
                        Forename = $"TestFirstName{index + 1}",
                        PayrollId = $"{index + 2}"
                    };
                    employees.Add(employee);
                }

                var employeeArray = employees.ToArray();
                var whereCondition = $"(({string.Join(") OR (", employeeArray.Select(s => $"Forename = '{s.Forename}' OR PayrollId = '{s.PayrollId}'"))}))";
                using (var context = new EmployeeContext())
                {
                    var testRecords = context.Employees.ToList();
                    var records = context.Employees.SqlQuery($"SELECT * FROM dbo.Employees WHERE {whereCondition}").ToList();
                    //var records = context.Employees.Join(employees, 
                    //        (emp) => new { emp.Forename, emp.PayrollId }, 
                    //        (empL) => new { empL.Forename, empL.PayrollId },
                    //        (emp, empL)=>new { emp, empL })
                    //        .Select(w=>w.emp).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Oh No!!!!");
                Console.ResetColor();
            }
        }

        private static void DeleteEmployees(EmployeeContext context, int records)
        {
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
        }

        private static void UpdateEmployees(EmployeeContext context, int records)
        {
            var employeesToUpdate = new List<Employee>();
            for (var index = 0; index < records; index++)
            {
                var employee = new Employee
                {
                    Id = index + 1,
                    Forename = $"UpdatedTestFirstName{index + 1}",
                    Surname = $"UpdatedTestSecondName{index + 1}",
                    JoiningDate = DateTime.Now,
                    MiddleName = $"UpdatedTestMiddleName{index + 1}",
                    PayrollId = $"{index + 1}"
                };
                employeesToUpdate.Add(employee);
            }
            var updateStopwatch = new Stopwatch();
            updateStopwatch.Start();
            //context.Employees.UpdateFromQuery(emp => new Employee
            //{
            //    Forename = $"UpdatedTestFirstName",
            //    Surname = $"UpdatedTestSecondName",
            //    JoiningDate = DateTime.Now,
            //    MiddleName = $"UpdatedTestMiddleName",
            //    PayrollId = $"1"
            //}, op => op.BatchTimeout = 60);
            context.BulkUpdate(employeesToUpdate);
            updateStopwatch.Stop();
            Console.WriteLine($"Total Time Taken To Update {records} records: {updateStopwatch.ElapsedMilliseconds}");
        }

        private static void InsertEmployee(EmployeeContext context, int records)
        {
            var employees = new List<Employee>();
            for (var index = 0; index < records; index++)
            {
                var employee = new Employee
                {
                    Id = index + 1,
                    Forename = $"TestFirstName{index + 1}",
                    Surname = $"TestSecondName{index + 1}",
                    JoiningDate = DateTime.Now,
                    MiddleName = $"TestMiddleName{index + 1}",
                    PayrollId = $"{index + 1}"
                };
                employees.Add(employee);
            }
            var insertStopwatch = new Stopwatch();
            insertStopwatch.Start();
            context.BulkInsert(employees);
            insertStopwatch.Stop();
            Console.WriteLine($"Total Time Taken To Insert {records} records: {insertStopwatch.ElapsedMilliseconds}");
        }

        private static void InsertDepartments(EmployeeContext context, int records)
        {
            var departmentList = new List<Department>();
            for (var index = 0; index < records; index++)
            {
                var department = new Department
                {
                    Id = index + 1,
                    Name = $"Department{index + 1}",
                    Description = $"Description{index + 1}"
                };
                departmentList.Add(department);
            }
            var insertStopwatch = new Stopwatch();
            insertStopwatch.Start();
            context.BulkInsert(departmentList);
            insertStopwatch.Stop();
            Console.WriteLine($"Total Time Taken To Insert {records} records: {insertStopwatch.ElapsedMilliseconds}");
        }
    }

    public class TestMe
    {
        public string Forename { get; set; }
        public string PayrollId { get; set; }
    }


}
