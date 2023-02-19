using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task1
{
    abstract class FileWatcher //fuck you, even if you fail, you can try later or
                               //another internship, you know. dont burn yourself, fucking phoenix
    {
        protected string formatRegEx;
        protected FileSystemWatcher watcher;
        public void createFileWatcher()
        {
            watcher = new FileSystemWatcher();
            watcher.Path = ConfigurationManager.AppSettings["MonitorDir"];
            watcher.IncludeSubdirectories = true;
            watcher.NotifyFilter = NotifyFilters.Attributes |
            NotifyFilters.CreationTime |
            NotifyFilters.DirectoryName |
            NotifyFilters.FileName |
            NotifyFilters.LastAccess |
            NotifyFilters.LastWrite |
            NotifyFilters.Security |
            NotifyFilters.Size;
            watcher.Filter = formatRegEx;
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;

        }
        public static void OnChanged(object source, FileSystemEventArgs e)
        {
            Console.WriteLine("{0}, with path {1} has been {2}", e.Name, e.FullPath, e.ChangeType);
        }

        public static void Process(string filePath)
        {
            var filename = filePath;//"C:\\_Path_of_prog\\task1Radency\\qwerty.txt";
            var lines = File.ReadAllLines(filename);
            var models = lines.Select(p => input.parseInput(p)).ToList().Where(m => m != null);
            var city = models.GroupBy(m => m.city);
            foreach (var c in city)
            {
                Console.WriteLine("city " + c.Key + "   " + c);
            }

            var cities = from m in models
                         group m by m.city into c
                         select new
                         {
                             cityName = c.Key,
                             total = c.Sum(m => m.payment),
                             services = from p in c
                                        group p by p.service into serv
                                        select new
                                        {
                                            serviceName = serv.Key,

                                            total = serv.Sum(p => p.payment),
                                            payers = from payer in serv select payer
                                        }
                         };
            using StreamWriter file = new("C:\\_Path_of_prog\\task1Radency\\output.txt");
            file.WriteLine("[");
            foreach (var c in cities)
            {
                file.WriteLine("  {\"city\": \"" + c.cityName + "\",");
                file.WriteLine("  \"services\": [");
                Console.WriteLine(c.cityName + "   total:" + c.total);
                foreach (var s in c.services)
                {
                    file.WriteLine("    {\"name\": \"" + s.serviceName + "\",");
                    file.WriteLine("    \"payers\": [");
                    Console.WriteLine(s.serviceName + "   total:" + s.total + "    "); ;
                    foreach (var p in s.payers)
                    {
                        file.WriteLine("      {\"name\": \"" + p.firstName+ " " + p.lastName + "\",");
                        file.WriteLine("      \"payment\": \"" + p.payment + "\",");
                        file.WriteLine("      \"date\": \"" + p.date + "\",");
                        file.WriteLine("      \"account_number\": \"" + p.accNumber+ "\"}");
                        Console.WriteLine(p.firstName + "   " + p.lastName + "   " + p.accNumber + "   " + p.payment);
                    }
                    file.WriteLine("    ]");
                    file.WriteLine("    \"total\": \"" + s.total + "\"}");
                }
                file.WriteLine("  ]");
                file.WriteLine("  \"total\": \"" + c.total + "\"}");
                file.WriteLine("]");
            }

          /*[{
              "city": "string",
              "services": [{
                "name": "string",
                "payers": [{
                  "name": "string",
                  "payment": "decimal",
                  "date": "date",
                  "account_number": "long"
                }],
                "total": "decimal"
              }],
              "total": "decimal"
            }]
*/


            /* var cityes = models.GroupBy(m => m.city).Select(city => new City {
                 cityName = city.Key,
                 total = city.Sum(m => m.payment),
                 services = city.GroupBy(c => city.service).Select(c => new service
                 {
                     serviceName = c.Key,
                     total = c.Sum(c => c.payment),

                 })
             }); ;*/
            foreach (input i in models)
            {
                Console.WriteLine("fuc");
            }
        }
       
        
    }

    class TxtFileWatcher : FileWatcher
    {
        public TxtFileWatcher()
        {
            base.formatRegEx = "*.txt";
            base.createFileWatcher();

        }
        public static void OnChanged(object source, FileSystemEventArgs e)
        {
            Process(e.FullPath);
            Console.WriteLine("{0}, with path {1} has been {2}", e.Name, e.FullPath, e.ChangeType);
        }
    }

    class CsvFileWatcher : FileWatcher
    {
        public CsvFileWatcher()
        {
            base.formatRegEx = "*.csv";
            base.createFileWatcher();
        }
        public string[] readFile(string fileAddress)
        {
            using (var reader = new StreamReader(fileAddress))
            {
                reader.ReadLine();
                string text = reader.ReadToEnd();
                return text.Split("\n");
            }

        }

        public void processInput(string[] lines)
        {
            var models = lines.Select(p => input.parseInput(p)).ToList().Where(m => m != null);
            return;
        }
    }
}
