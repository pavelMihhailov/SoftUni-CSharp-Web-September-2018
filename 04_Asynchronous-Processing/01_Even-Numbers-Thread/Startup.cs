using System;
using System.Linq;
using System.Threading;

namespace _04_Asynchronous_Processing
{
    public class Startup
    {
        public static void Main()
        {
            int[] range = Console.ReadLine().Split(' ').Select(x => int.Parse(x)).ToArray();

            var thread = new Thread(() =>
            {
                for (int i = range[0]; i <= range[1]; i++)
                {
                    if (i % 2 == 0)
                    {
                        Console.WriteLine(i);
                    }
                }
            });

            thread.Start();
            thread.Join();
            Console.WriteLine("Thread finished work!");
        }
    }
}
