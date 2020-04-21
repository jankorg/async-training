
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Utils;

namespace Tasks
{
    class Program
    {
        /*Files procesor using Tasks */
        static string path = @"C:\Users\Public\Documents\endava\Tasks";

        static void Main(string[] args)
        {
            var collection = new BlockingCollection<string>(4);
            var cdEvnt = new CountdownEvent(10);
            var cancelTokenSource = new CancellationTokenSource();
            var util = new Util();

            var publisher = new Publisher(collection, cancelTokenSource.Token);
            var consumer = new Consumer(collection, cdEvnt, cancelTokenSource.Token);

            publisher.Start(path);
            Task.Run(() => consumer.Start(), cancelTokenSource.Token);

            cdEvnt.Wait(cancelTokenSource.Token);
            cancelTokenSource.Cancel();

            var data = util.GetData();
            Parallel.ForEach(data, item =>
            {
                Console.WriteLine($"{item.Key} - {item.Value}");
            });

        }
    }
}
