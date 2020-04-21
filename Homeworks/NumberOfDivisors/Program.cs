using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NumberOfDivisors
{
    class Program
    {
        static void Main(string[] args)
        {
            var numThreads = 8;
            int range = 100;
            var totalNumbers = range / numThreads;
            var array = BuildRandomArray(range);

            var stopwatch = Stopwatch.StartNew();

            int i = 0;
            var arrList = from item in array
                         group item by i++ % numThreads into part
                         select part.AsEnumerable();


            var arrs = new List<IEnumerable<int>>();
            foreach (var item in arrList)
            {
                arrs.Add(item.ToArray());
            }


            var ListOfProcessNumb = new List<NumberProcessor>();

            foreach (var item in arrs)
            {
                ListOfProcessNumb.Add(new NumberProcessor(item.ToArray(), totalNumbers));

            }

            var listThread = new List<Thread>();

            foreach (var item in ListOfProcessNumb)
            {
                listThread.Add(new Thread(item.CalculateDivisor));

            }

            foreach (var item in listThread)
            {
                item.Start();
            }


            Console.WriteLine("Processing Threads...");

            listThread.Reverse();

            foreach (var item in listThread)
            {
                item.Join();
            }


            var numSum = new List<Result>();
            foreach (var item in ListOfProcessNumb)
            {
                numSum.AddRange(item.results);
            }


            stopwatch.Stop();

            Console.WriteLine($"Elapsed time: {stopwatch.Elapsed.TotalMilliseconds} ms");
            var result = numSum.First(n => n.Amount == numSum.Max(m => m.Amount));

            Console.WriteLine($"most number: {result.MostNumber}, ran {result.Amount} times.");
            Console.ReadLine();

        }

        public static int[] BuildRandomArray(int size)
        {

            int Min = 0;
            int Max = 100; 

            int[] arr = new int[size];

            Random randNum = new Random();
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = randNum.Next(Min, Max);
            }

            return arr;
        }
    }
}