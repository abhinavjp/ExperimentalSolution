using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA.PracticeService.DataStructure
{
    public static class LinkedListPractice
    {
        public static void Run()
        {
            Console.Clear();
            var lnklist = new LinkedList();
            lnklist.PrintAllNodes();
            Console.WriteLine();

            lnklist.AddAtLast(12);
            lnklist.AddAtLast("John");
            lnklist.AddAtLast("Peter");
            lnklist.AddAtLast(34);
            lnklist.PrintAllNodes();
            Console.WriteLine();

            lnklist.AddAtStart(55);
            lnklist.PrintAllNodes();
            Console.WriteLine();

            lnklist.RemoveFromStart();
            lnklist.PrintAllNodes();
        }
    }
}
