namespace WordGenerator
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    internal class Program
    {
        private const int NrFiles = 10;
        private const int NrWordsOnEachFile = 1000000;

        private static readonly char[] Cons =
            {'b', 'c', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'm', 'n', 'p', 'q', 'r', 's', 't', 'v', 'w', 'x', 'z'};

        private static readonly char[] Vowel = {'a', 'e', 'i', 'o', 'u', 'y'};

        private static void Main(string[] args)
        {
            var tl = new List<Task>();

            for (var i = 0; i < NrFiles; i++) tl.Add(GenerateWords(i));

            Task.WaitAll(tl.ToArray());

            Console.WriteLine("Finished");
        }

        private static Task GenerateWords(int fileId)
        {
            return Task.Run(() =>
            {
                var file = $"file.{fileId}.dat";

                long nrOfWords = NrWordsOnEachFile;

                var lines = new string[nrOfWords];

                var rand = new Random(100);

                for (var idx = 0; idx < nrOfWords; idx++) lines[idx] = GenerateWord(rand, rand.Next(1, 20));

                // Write the string array to a new file named "WriteLines.txt".
                using (var outputFile = new StreamWriter(file))
                {
                    foreach (var line in lines) outputFile.WriteLine(line);
                }
            });
        }

        private static string GenerateWord(Random rand, int length)
        {
            if (length < 1) // do not allow words of zero length
                throw new ArgumentException("Length must be greater than 0");

            var word = string.Empty;

            if (rand.Next() % 2 == 0) // randomly choose a vowel or consonant to start the word
            {
                word += Cons[rand.Next(0, 20)];
            }
            else
            {
                word += Vowel[rand.Next(0, 4)];
            }

            for (var i = 1; i < length; i += 2) // the counter starts at 1 to account for the initial letter
            {
                // and increments by two since we append two characters per pass
                var c = Cons[rand.Next(0, 20)];
                var v = Vowel[rand.Next(0, 4)];

                word += c + v.ToString();
            }

            // the word may be short a letter because of the way the for loop above is constructed
            if (word.Length < length) // we'll just append a random consonant if that's the case
            {
                word += Cons[rand.Next(0, 20)];
            }

            return word;
        }
    }
}
