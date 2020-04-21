using System;
using System.Collections.Concurrent;

namespace Utils
{
    public class Util
    {
        private readonly ConcurrentDictionary<string, string> data;

        public Util()
        {
            this.data = new ConcurrentDictionary<string, string>();
        }

        public ConcurrentDictionary<String, String> GetData()
        {

            return this.data;
        }
    }
}
