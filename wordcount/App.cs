using System;
using System.Collections.Generic;
using System.IO;

namespace wordcount
{
    public class App
    {
        public App()
        {
        }

        public void Run()
        {
            WordCounter counter = new WordCounter();
            WordDatabase db = new WordDatabase();

            var root = new DirectoryInfo(@"/Users/ole/data");

            DateTime start = DateTime.Now;

            var res = counter.CountWords(root, new List<string> { ".txt"});

          

            foreach (var p in res)
            {
                db.Insert(p.Key, p.Value);
            }

            TimeSpan used = DateTime.Now - start;
            Console.WriteLine("DONE! used " + used.TotalMilliseconds);

            var all = db.GetAll();

            foreach (var p in all)
            {
                Console.WriteLine("<" + p.Key + ", " + p.Value + ">");
                break;
            }


        }


    }
}
