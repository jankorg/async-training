using Homeworks;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Utils;

namespace Threads
{
    internal class Program
    {
        /*Files procesor using Threads */

        static string path = @"C:\Users\Public\Documents\endava\Threads";
        public static void Main(string[] args)
        {
            var collection = new BlockingCollection<string>(4);
            var cdEvnt = new CountdownEvent(10);
            var publisher = new Publisher(collection);
            var consumer = new Consumer(collection, cdEvnt);
            var util = new Util();

            Thread thPublisher = new Thread(() => publisher.Start(path));
            Thread thConsumer = new Thread(consumer.Start);

            thPublisher.Start();
            thConsumer.Start();

            thPublisher.Join();
            thConsumer.Join();

            cdEvnt.Wait();

            var data = util.GetData();
            Parallel.ForEach(data, item =>
            {
                Thread thread = new Thread(() => Console.WriteLine($"{item.Key} - {item.Value}"));
                thread.Start();
            });

        }

    }

}
