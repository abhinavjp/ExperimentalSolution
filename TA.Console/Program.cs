using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TA.PracticeService.DataStructure;
using TA.PracticeService.EF_Bulk_Practice;

namespace TA.ConsoleApp
{
    class Program
    {
        public static void Main(string[] args)
        {
            LinkedListPractice.Run();
            StackPractice.Run();

            EfBulkPractice.Run();

            Console.ReadKey();
        }
    }
}
