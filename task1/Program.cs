using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace task1
{
    class Program
    {
        public static int ParsedFiles = 0;
        public static long ParsedStrings = 0;
        public static long FoundErrors = 0;
        public static List<string> InvalidFiles = new List<string> { };


        static void Main(string[] args)
        {
            Console.WriteLine("Press s to start");
            while (Console.Read() != 's') ;
            if (! System.IO.File.Exists(System.Reflection.Assembly.GetEntryAssembly().Location + ".config")){
                return;
            }

            
            LogTimer l = new LogTimer();
            l.Init();
            TxtFileWatcher txtWatcher = new TxtFileWatcher();
            CsvFileWatcher csvWatcher = new CsvFileWatcher();
            Console.WriteLine("Process started");
            Console.WriteLine("Press q to quit.\nPress r to restart");
            int input=Console.Read();
            while (true)
            {
                if (input == 'q') break;
                if (input == 'r') {
                    var fileName = Assembly.GetExecutingAssembly().Location;
                    System.Diagnostics.Process.Start(fileName);
                    break;
                }
                input = Console.Read();
            }
            
        }

        
    }
}
