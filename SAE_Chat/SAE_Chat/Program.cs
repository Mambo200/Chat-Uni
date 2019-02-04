using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAE_Chat
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] bytes = new byte[1024 * 10];
            short length = 5000;
            bytes[0] = (byte)length;
            bytes[1] = (byte)(length >> 8);
            Console.WriteLine(bytes[0] + (bytes[1] << 8));
            Console.ReadKey();
        }
    }
}
