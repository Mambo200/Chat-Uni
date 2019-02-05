using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Threading
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread thread = new Thread(Foo);
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            thread.Name = "Foo Thread";
            thread.Start();
        }

        static void Foo()
        {
            for (int i = 0; i < 10000; i++)
            {
                Console.WriteLine(i);
            }
        }
    }
}
