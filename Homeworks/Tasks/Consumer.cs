using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tasks
{
    class Consumer
    {
        private readonly BlockingCollection<string> consumerCol;
        private readonly CountdownEvent cdEvnt;
        private readonly ConcurrentDictionary<string, string> data;
        private readonly CancellationToken cancelToken;

        public Consumer(BlockingCollection<string> blockingCollection, CountdownEvent countdownEvent, CancellationToken cancellationToken)
        {
            this.consumerCol = blockingCollection;
            this.data = new ConcurrentDictionary<string, string>();
            this.cdEvnt = countdownEvent;
            this.cancelToken = cancellationToken;
        }

        public void Start()
        {

            while (!cancelToken.IsCancellationRequested)
            {
                    try
                    {
                        var result = this.consumerCol.TryTake(out var path, 500, cancelToken);
                        if (result)
                        {
                            try
                            {
                                var content = File.ReadAllText(path);
                                this.data.TryAdd(path, content);
                                this.cdEvnt.Signal();
                            }
                            catch (Exception)
                            {
                                Console.WriteLine($"Retry for {path} on {Thread.CurrentThread.ManagedThreadId}");
                                this.consumerCol.Add(path);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{ex.Message}");
                    }
            }
        }

    }
}
