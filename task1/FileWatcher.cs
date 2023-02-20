﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task1
{
    abstract class FileWatcher 
    {
        protected string formatRegEx;
        protected FileSystemWatcher watcher;
        public void CreateFileWatcher()
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


        }
        public static void OnChanged(object source, FileSystemEventArgs e){}

        public static string[] ReadFile(string filePath) { return null; }
        public static void Process(string[] lines, string filePath)
        {
            ++Program.ParsedFiles;
            var models = lines.Select(p => Input.ParseInput(p, filePath)).ToList().Where(m => m != null);
            var city = models.GroupBy(m => m.city);


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
            string dir = ConfigurationManager.AppSettings["OutputDir"] + "\\" + DateTime.Now.ToString("yyyy-dd-MM");
            System.IO.Directory.CreateDirectory(dir);
            using (StreamWriter file = new(dir  + "\\output"+Program.ParsedFiles+".json"))
            {            
                  file.WriteLine("[");
                  foreach (var c in cities)
                  {
                      file.WriteLine("  {\"city\": \"" + c.cityName + "\",");
                      file.WriteLine("  \"services\": [");
                      foreach (var s in c.services)
                      {
                          file.WriteLine("    {\"name\": \"" + s.serviceName + "\",");
                          file.WriteLine("    \"payers\": [");
                          foreach (var p in s.payers)
                          {
                              file.WriteLine("      {\"name\": \"" + p.firstName + " " + p.lastName + "\",");
                              file.WriteLine("      \"payment\": \"" + p.payment + "\",");
                              file.WriteLine("      \"date\": \"" + p.date.Date + "\",");
                              file.WriteLine("      \"account_number\": \"" + p.accNumber + "\"}");
                          }
                          file.WriteLine("    ]");
                          file.WriteLine("    \"total\": \"" + s.total + "\"}");
                      }
                      file.WriteLine("  ]");
                      file.WriteLine("  \"total\": \"" + c.total + "\"}");

                  }
                  file.WriteLine("]");
                file.Flush();
                file.Dispose();
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

        }
       
        
    }

    class TxtFileWatcher : FileWatcher
    {
        public TxtFileWatcher()
        {
            base.formatRegEx = "*.txt";
            base.CreateFileWatcher();
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;
        }

        public static new string[] ReadFile(string filePath)
        {
            var filename = filePath;
            return File.ReadAllLines(filename);
        }
        public static void OnChanged(object source, FileSystemEventArgs e)
        {
            string[] input = TxtFileWatcher.ReadFile(e.FullPath);
            FileWatcher.Process(input, e.FullPath);
        }
    }

    class CsvFileWatcher : FileWatcher
    {
        public CsvFileWatcher()
        {
            base.formatRegEx = "*.csv";
            base.CreateFileWatcher();
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;
        }
        public static new string[] ReadFile(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                reader.ReadLine();
                string text = reader.ReadToEnd();
               
                return text.Split("\n");
            }

        }
        public static new void OnChanged(object source, FileSystemEventArgs e)
        {
            string[] input = CsvFileWatcher.ReadFile(e.FullPath);
            FileWatcher.Process(input, e.FullPath);
        }


    }
}
