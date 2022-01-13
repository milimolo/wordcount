using System;
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

            var root = new DirectoryInfo(@"/Users/ole/data/arnold-j");

            var res = counter.Crawl(root);

          

            foreach (var p in res)
            {
                db.Insert(p.Key, p.Value);
            }

            Console.WriteLine("DONE!");

            foreach (var p in db.GetAll())
            {
                Console.WriteLine("<" + p.Key + ", " + p.Value + ">");
            }


        }


    }
}
