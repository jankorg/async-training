using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;

namespace Homeworks
{
    public class Consumer
    {
        private readonly BlockingCollection<string> consumerCol;
        private readonly CountdownEvent cdEvnt;
        private readonly ConcurrentDictionary<string, string> data;

        public Consumer(BlockingCollection<string> blCollection, CountdownEvent cdEvnt)
        {
            this.consumerCol = blCollection;
            this.cdEvnt = cdEvnt;
            this.data = new ConcurrentDictionary<string, string>();
        }

        public void Start()
        {
            for (int lim = 0; cdEvnt.InitialCount > lim; lim += 1)
            {
                var result = this.consumerCol.TryTake(out var path, 500);
                if (result)
                {
                    try
                    {
                        var content = File.ReadAllText(path);
                        this.data.TryAdd(path, content);
                        this.cdEvnt.Signal();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{ex.Message} in: Path: {path} ThreadId: {Thread.CurrentThread.ManagedThreadId}");
                        this.consumerCol.Add(path);
                    }
                }
            }

        }

    }
}
