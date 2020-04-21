using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Tasks
{
    class Publisher
    {
        private readonly BlockingCollection<string> pubCollection;
        private readonly CancellationToken cancelToken;

        public Publisher(BlockingCollection<string> blockingCollection, CancellationToken cancellationToken)
        {
            this.pubCollection = blockingCollection;
            this.cancelToken = cancellationToken;
        }

        public void Start(string path)
        {
            FileSystemWatcher watcher = new FileSystemWatcher(path);
            watcher.Path = path;
            watcher.NotifyFilter = NotifyFilters.FileName |
                                    NotifyFilters.LastAccess |
                                    NotifyFilters.LastWrite |
                                    NotifyFilters.DirectoryName;

            watcher.Created += (sender, args) =>
            {
                Console.WriteLine($"File read on {Task.CurrentId}");
                this.pubCollection.Add(args.FullPath, cancelToken);
            };
            watcher.EnableRaisingEvents = true;

        }
    }
}
