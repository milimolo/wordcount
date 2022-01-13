using System;
using System.IO;
using Microsoft.Data.Sqlite;

namespace wordcount
{
    class Program
    {
        static void Main(string[] args)
        {
            new App().Run();

            //new Renamer().Crawl(new DirectoryInfo(@"/Users/ole/data/arnold-j"));


        }

        static void crawlerSpike()
        {
            var root = new DirectoryInfo(@"/Users/ole/data/arnold-j/bmc");
            Crawl(root);


        }

        static void Crawl(DirectoryInfo dir)
        {
            Console.WriteLine("Crawling " + dir.FullName);
            foreach (var file in dir.EnumerateFiles()) HandleFile(file);

            foreach (var d in dir.EnumerateDirectories())
                Crawl(d);
        }

        private static void HandleFile(FileInfo file)
        {
            if (file.Name.StartsWith('.')) return;
            Console.WriteLine("Handling file: " + file.FullName);
            var content = File.ReadAllLines(file.FullName);
            int lineNo = 1;
            foreach (var line in content)
            {
                Console.WriteLine(lineNo.ToString() + ": " + line);
                lineNo++;
            }
        }



        static void SplitSpike()
        {
            String sep = " \n\t\",;.:-_**+=)([]{}/&%€#";

            //string line = "I do not know if you received a \"heads - up\" on this,but since you are working with Executive calendars and the";
            string line = "Basking Ridge either:  (for 1.5 days of a 3 day period) ";
            line += "December 19 - 21, 2000 or January 9 - 11, 2001 the(tentative) ";
            line += "preference leanings are toward rescheduling the currently proposed dates of December 13 / 14 to the January 2001 dates.";
            Console.WriteLine("Split on ");
            Console.WriteLine(line);
            Console.WriteLine("---");
            foreach (var w in line.Split(sep.ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                Console.WriteLine(w);
            }
        }

        static void DBSpike()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder();

            //Use DB in project directory.  If it does not exist, create it:
            connectionStringBuilder.DataSource = "./SqliteDB.db";

            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {

                //SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
                connection.Open();

                //Create a table (drop if already exists first):

                var delTableCmd = connection.CreateCommand();
                delTableCmd.CommandText = "DROP TABLE IF EXISTS favorite_beers";
                delTableCmd.ExecuteNonQuery();

                var createTableCmd = connection.CreateCommand();
                createTableCmd.CommandText = "CREATE TABLE favorite_beers(id INTEGER PRIMARY KEY AUTOINCREMENT, name VARCHAR(50), count INTEGER)";
                createTableCmd.ExecuteNonQuery();

                //Seed some data:
                using (var transaction = connection.BeginTransaction())
                {
                    var insertCmd = connection.CreateCommand();

                    insertCmd.CommandText = "INSERT INTO favorite_beers(name, count) VALUES('LAGUNITAS IPA', 5)";
                    insertCmd.ExecuteNonQuery();

                    insertCmd.CommandText = "INSERT INTO favorite_beers(name, count) VALUES('JAI ALAI IPA', 3)";
                    insertCmd.ExecuteNonQuery();

                    insertCmd.CommandText = "INSERT INTO favorite_beers(name, count) VALUES('RANGER IPA', 1)";
                    insertCmd.ExecuteNonQuery();

                    transaction.Commit();
                }

                //Read the newly inserted data:
                var selectCmd = connection.CreateCommand();
                selectCmd.CommandText = "SELECT * FROM favorite_beers";

                using (var reader = selectCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = reader.GetInt32(0);
                        var message = reader.GetString(1);
                        var c = reader.GetInt32(2);
                        Console.WriteLine("id=" + id + ": " + message + " -- " + c);
                    }
                }


            }
        }
    }
}