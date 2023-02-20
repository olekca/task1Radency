using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace task1
{
    class LogTimer
    {
        static Timer timer;
        long interval = 30000; //30 секунд
        int Hour = 21;
        int Minute = 35;
        static object synclock = new object();
        static bool isLogged = false;

        public void Init()
        {
            timer = new Timer(new TimerCallback(MakeLog), null, 0, interval);
        }

        void cleanLog()
        {
            Program.FoundErrors = 0;
            Program.InvalidFiles = new List<string> { };
            Program.ParsedFiles = 0;
            Program.ParsedStrings = 0;
        }
        private void MakeLog(object obj)
        {
            lock (synclock)
            {
                DateTime dd = DateTime.Now;
                if (dd.Hour == Hour && dd.Minute == Minute && isLogged == false)
                {
                    string dir = ConfigurationManager.AppSettings["OutputDir"] + "\\" + DateTime.Now.ToString("yyyy-dd-MM");
                    System.IO.Directory.CreateDirectory(dir);
                    using (StreamWriter file = new(dir + "\\meta.log"))
                    {
                        file.WriteLine("parsed_files:" + Program.ParsedFiles);
                        file.WriteLine("parsed_lines:" + Program.ParsedStrings);
                        file.WriteLine("found_errors:" + Program.FoundErrors);
                        file.Write("invalid_files: [");
                        bool once = true;
                        foreach(string s in Program.InvalidFiles)
                        {
                            if (once == false)
                            {
                                file.Write(", ");
                            }
                            else
                            {
                                once = false;
                            }
                            file.Write(s);
                        }
                        file.Write("]");
                    }
                    cleanLog();
                }
                else if (dd.Hour != Hour && dd.Minute != Minute)
                {
                    isLogged = false;
                }
            }
        }
        public void Dispose()
        { }
    }
}
