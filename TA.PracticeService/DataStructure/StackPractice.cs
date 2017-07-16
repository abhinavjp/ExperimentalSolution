using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA.PracticeService.DataStructure
{
    public static class StackPractice
    {
        public static void Run()
        {
            Console.Clear();
            var stack = new Stack();
            stack.Push(12);


            stack.Push("John");
            stack.Push("Peter");
            stack.Push(34);
            stack.Display();
            Console.WriteLine();

            stack.Push(55);
            stack.Display();
            Console.WriteLine();

            var poppedElement = stack.Pop();
            Console.WriteLine($"Popped Element: {poppedElement}");
            stack.Display();

            var peekedElement = stack.Peek();
            Console.WriteLine($"Peeked Element: {peekedElement}");
            stack.Display();
        }
    }
}
