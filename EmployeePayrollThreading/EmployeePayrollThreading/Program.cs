using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EmployeePayrollThreading
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("---------------------This is EmployeePayroll Threading--------------------");
            string[] words = CreateWordArray(@"http://www.gutenberg.org/files/54700/54700-0.txt");
            Parallel.Invoke(() =>
            {
                Console.WriteLine("Begin first task");
                GetLongestWord(words);
            },
            () =>
            {
                Console.WriteLine("Begin second task");
                GetMostCommonWords(words);
            },
            () =>
            {
                Console.WriteLine("begin third task");
                GetCountForWord(words, "sleep");
            }
            );
            Console.ReadKey();
        }

        public static void GetMostCommonWords(string[] words)
        {
            var frequencyOrder = from word in words
                                 where word.Length > 6
                                 group word by word into q
                                 orderby q.Count() descending
                                 select q.Key;
            var commonWord = frequencyOrder.Take(10);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Task 2 -- The most common words are : ");
            foreach (var v in commonWord)
            {
                sb.AppendLine(" " + v);
            }
            Console.WriteLine(sb.ToString());
        }


        public static string[] CreateWordArray(string uri)
        {
            Console.WriteLine("Retrieving from {0}", uri);
            string blog = new WebClient().DownloadString(uri);
            return blog.Split(
                new char[] { ' ', '\u000A', '.', ',', '-', '_', '/' },
                StringSplitOptions.RemoveEmptyEntries);
        }
        private static string GetLongestWord(string[] words)
        {
            string longestWord = (from w in words
                                  orderby w.Length descending
                                  select w).First();
            Console.WriteLine("Task 1 - the longest word is {0}", longestWord);
            return longestWord;
        }
        static void GetCountForWord(string[] words, string term)
        {
            var findWord = from word in words where word.ToUpper().Contains(term.ToUpper()) select word;

            Console.WriteLine($@"Task 3 -- The word --{term}-- occurs {findWord.Count()} times.");
        }
    }
}
