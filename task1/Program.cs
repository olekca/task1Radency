using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace task1
{
    class Program
    {
        //variables for logging
        public static int ParsedFiles = 0;
        public static long ParsedStrings = 0;
        public static long FoundErrors = 0;
        public static List<string> InvalidFiles = new List<string> { };

        public static TxtFileWatcher txtWatcher; 
        public static CsvFileWatcher csvWatcher;
        public  static LogTimer Log;
        static void Main(string[] args)
        {
            Console.WriteLine("Press s to start");
            while (Console.Read() != 's') ;
            
            //checks config
            if (ConfigurationManager.AppSettings["MonitorDir"]==null)
            {
                Console.WriteLine("Config settings not found");
                return;
            }


            Start();
            Console.WriteLine("Process started");
            Console.WriteLine("Press q to quit.\nPress r to restart");
            int input=Console.Read();
            while (true)
            {
                if (input == 'q') break;
                if (input == 'r') {
                    Start();
                    Console.WriteLine("Successfully restarted");
                    Console.WriteLine("Press q to quit.\nPress r to restart");
                }
                input = Console.Read();
            }
            
        }
        public static void Start()
        {
            Log = new LogTimer();
            Log.Init();
            txtWatcher = new TxtFileWatcher();
            csvWatcher = new CsvFileWatcher();
            CleanLog();
        }
        public static void CleanLog()
        {
            Program.FoundErrors = 0;
            Program.InvalidFiles = new List<string> { };
            Program.ParsedFiles = 0;
            Program.ParsedStrings = 0;
        }

    }
}
