using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using Homeworks;


namespace Homeworks
{
    public class Publisher
    {
        private readonly BlockingCollection<string> pubCollection;

        public Publisher(BlockingCollection<string> blCollection)
        {
            this.pubCollection = blCollection;
        }

        public void Start(string path)
        {
            FileSystemWatcher fw = new FileSystemWatcher(path);
            fw.Path = path;
            fw.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.DirectoryName;

            fw.Created += (sender, args) =>
            {
                Console.WriteLine($"Thread ID: {Thread.CurrentThread.ManagedThreadId}");
                Console.ReadLine();
                this.pubCollection.Add(args.FullPath);
            };
            fw.EnableRaisingEvents = true;
        }
    }
}
