using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberOfDivisors
{
    public class NumberProcessor
    {
        private readonly int[] array;
        private readonly int totalNumbers;

        public NumberProcessor(int[] array, int totalNumbers)
        {
            this.array = array;
            this.totalNumbers = totalNumbers;
            this.data = new Dictionary<string, int>();
            this.results = new List<Result>();
        }

        private Dictionary<string, int> data { get; set; }

        public List<Result> results { get; set; }

        public void CalculateDivisor()
        {

            try
            {
                for (int i = 0; i <= array.Length - 1; i++)
                {
                    int number = array[i];
                    if (number != 0)
                    {
                        int j = 1;
                        int k = 1;

                        while (j <= totalNumbers)
                        {
                            if (number % j == 0)
                            {
                                if (data.Any(d => d.Key == number.ToString()))
                                {
                                    data.Remove(number.ToString());
                                    data.Add(number.ToString(), k); ;
                                }
                                else
                                {
                                    data.Add(number.ToString(), k);
                                }

                                k++;
                            }
                            else
                            {
                                if (j > number)
                                {
                                    j = totalNumbers;
                                }
                            }

                            j++;
                        }
                    }
                }

                GroupMostNumber(data);
            }
            catch (Exception ex)
            {
                Console.Write($"Error: {ex.Message}");
            }
        }

        private void GroupMostNumber(Dictionary<string, int> data)
        {

            var list = (data
                 .GroupBy(d => d.Key)
                 .Select(g => new
                 {
                     MostNumber = g.Key,
                     Amount = g.Sum(x => x.Value)
                 })).ToList();

            foreach (var item in list)
            {
                Result r = new Result() { MostNumber = item.MostNumber, Amount = item.Amount };
                results.Add(r);
            }

        }
    }

    public class Result
    {
        public string MostNumber { get; set; }
        public int Amount { get; set; }
    }
}
