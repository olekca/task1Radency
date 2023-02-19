using System;
using System.Configuration;
using System.IO;
namespace task1
{
    class Program
    {
       
        static void Main(string[] args)
        {
            TxtFileWatcher w = new TxtFileWatcher();
            FileWatcher.Process("C:\\_Path_of_prog\\task1Radency\\qwerty.txt");
            /*
            FileSystemWatcher watcher = new FileSystemWatcher();
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
            watcher.Filter = "*.*";
            // Add event handlers.  
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Deleted += new FileSystemEventHandler(OnChanged);
            watcher.Renamed += new RenamedEventHandler(OnRenamed);
            //Start monitoring.  
            watcher.EnableRaisingEvents = true;
            //Do some changes now to the directory.  
            //Create a DirectoryInfo object.  
            DirectoryInfo d1 = new DirectoryInfo(ConfigurationManager.AppSettings["MonitorDir"]);
            //Create a new subdirectory.  
            d1.CreateSubdirectory("mydir");
            //Create some subdirectories.  
            d1.CreateSubdirectory("mydir1\\mydir2\\mydir3");
            //Move the subdirectory "mydir3 " to "mydir\mydir3"  
            Directory.Move(d1.FullName + "file://mydir1//mydir2//mydir3",
            d1.FullName + "\\mydir\\mydir3");
            //Check if subdirectory "mydir1" exists.  
            if (Directory.Exists(d1.FullName + "\\mydir1"))
            {
                //Delete the directory "mydir1"  
                //I have also passed 'true' to allow recursive deletion of  
                //any subdirectories or files in the directory "mydir1"  
                Directory.Delete(d1.FullName + "\\mydir1", true);
            }
            //Get an array of all directories in the given path.  
            DirectoryInfo[] d2 = d1.GetDirectories();
            //Iterate over all directories in the d2 array.  
            foreach (DirectoryInfo d in d2)
            {
                if (d.Name == "mydir")
                {
                    //If "mydir" directory is found then delete it recursively.  
                    Directory.Delete(d.FullName, true);
                }
            }
            // Wait for user to quit program.  */
            Console.WriteLine("Press \'q\' to quit the sample.");
            Console.WriteLine();
            //Make an infinite loop till 'q' is pressed.  
            while (Console.Read() != 'q') ;
        }

        public static void OnChanged(object source, FileSystemEventArgs e)
        { 
            Console.WriteLine("{0}, with path {1} has been {2}", e.Name, e.FullPath, e.ChangeType);
        }
        public static void OnRenamed(object source, RenamedEventArgs e)
        {
            Console.WriteLine(" {0} renamed to {1}", e.OldFullPath, e.FullPath);
        }
    }
}
