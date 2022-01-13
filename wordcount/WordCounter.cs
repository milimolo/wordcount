using System;
using System.Collections.Generic;
using System.IO;

namespace wordcount
{
    public class WordCounter
    {
        private char[] sep = " \n\t\",;.:-_**+=)([]{}/&%€#".ToCharArray();

        public WordCounter()
        {
        }

        Dictionary<string, int> CountFile(FileInfo f)
        {
            Dictionary<string, int> res = new Dictionary<string, int>();
            var content = File.ReadAllLines(f.FullName);
            foreach (var line in content)
            {
                foreach (var aWord in line.Split(sep, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (res.ContainsKey(aWord))
                        res[aWord]++;
                    else
                        res.Add(aWord, 1);
                }
            }

            return res;
        }

        public Dictionary<string, int> Crawl(DirectoryInfo dir) {
            Dictionary<string, int> res = new Dictionary<string, int>();

            Console.WriteLine("Crawling " + dir.FullName);

            foreach (var file in dir.EnumerateFiles()) 
                AddDictionary(res, CountFile(file));
           
            foreach (var d in dir.EnumerateDirectories())
                AddDictionary(res, Crawl(d));

            return res;
        }

        private void AddDictionary(Dictionary<string, int> res, Dictionary<string, int> cFile)
        {
            foreach (KeyValuePair<String, int> p in cFile)
            {
                if (res.ContainsKey(p.Key))
                    res[p.Key] += p.Value;
                else
                    res.Add(p.Key, p.Value);
            }
        }
    }
}
