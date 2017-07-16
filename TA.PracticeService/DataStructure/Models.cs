using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA.PracticeService.DataStructure
{
    public class LinkedList
    {
        private Node head;
        private Node current;
        public int Count;
        public LinkedList()
        {
            head = new Node();
            current = head;
        }

        public void AddAtLast(object data)
        {
            var newNode = new Node
            {
                Value = data
            };
            current.Next = newNode;
            current = newNode;
            Count++;
        }

        public void AddAtStart(object data)
        {
            var newNode = new Node
            {
                Value = data,
                Next = head.Next//new node will have reference of head's next reference
            };
            head.Next = newNode;//and now head will refer to new node
            Count++;
        }

        public void RemoveFromStart()
        {
            if (Count > 0)
            {
                head.Next = head.Next.Next;
                Count--;
            }
            else
            {
                Console.WriteLine("No element exist in this linked list.");
            }
        }

        public void PrintAllNodes()
        {
            //Traverse from head
            Console.Write("Head ->");
            var curr = head;
            while (curr.Next != null)
            {
                curr = curr.Next;
                Console.Write(curr.Value);
                Console.Write("->");
            }
            Console.Write("NULL");
        }
    }
    public class Node
    {
        public Node Next;
        public object Value;
    }
    public class Stack
    {
        private const int DefaultStackSize = 10;
        public int Capacity { get; set; }
        private int _count;
        public int Count => _count;
        private int _top;
        Object[] item;
        public Stack()
        {
            Capacity = DefaultStackSize;
            item = new Object[Capacity];
            _top = -1;
        }

        public Stack(int capacity)
        {
            Capacity = capacity;
            item = new Object[Capacity];
            _top = -1;
        }

        public bool IsEmpty
        {
            get
            {
                if (_top == -1) return true;
                return false;
            }
        }

        public void Push(object element)
        {
            if (_top == (Capacity - 1))
            {
                Console.WriteLine("Stack is full!");
            }
            else
            {
                item[++_top] = element;
                _count++;
                Console.WriteLine("Item pushed successfully!");
            }
        }

        public object Pop()
        {
            if (IsEmpty)
            {
                Console.WriteLine("Stack is empty!");
                return "No elements";
            }
            else
            {
                _count--;
                return item[_top--];
            }
        }

        public object Peek()
        {
            if (IsEmpty)
            {
                Console.WriteLine("Stack is empty!");
                return "No elements";
            }
            else
            {
                return item[_top];
            }
        }


        public void Display()
        {
            for (int i = _top; i > -1; i--)
            {
                Console.WriteLine($"Item {(i + 1)}: {item[i]}");
            }
        }
    }
    public class Queue
    {
        public int Front;
        public int Rear;
        public int[] items;
        public Queue(int queueCapacity)
        {
            Front = 0;
            Rear = -1;
            items = new int[queueCapacity];
        }
    }
}
