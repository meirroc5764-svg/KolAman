using NotificationGate.Producer;
using NotificationGate.ReadFile;
using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {

        var read = new ReadFile();
        //  Create a FileSystemWatcher to monitor all files on drive C.
        FileSystemWatcher fsw = new FileSystemWatcher("C:\\Users\\Aenigma\\OneDrive\\Desktop\\alert-simulator\\alert-simulator\\alerts\\aman");

        //  Watch for changes in LastAccess and LastWrite times, and
        //  the renaming of files or directories.
        fsw.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName;

        //  Register a handler that gets called when a
        //  file is created, changed, or deleted.

        fsw.Created += (o, e) =>
        {
            // add a file to the queue
            read.ReadData(e.FullPath.ToString());
            Console.WriteLine("send a message");
            OnChanged(o, e);
        }
        ;


        //  Begin watching.
        Task.Delay(100);
        fsw.EnableRaisingEvents = true;

        Console.WriteLine("Press \'Enter\' to quit the sample.");
        Console.ReadLine();
    }

    //  This method is called when a file is created, changed, or deleted.
    private static void OnChanged(object source, FileSystemEventArgs e)
    {
        //  Show that a file has been created, changed, or deleted.
        WatcherChangeTypes wct = e.ChangeType;
        
        Console.WriteLine("File {0} {1}", e.FullPath, wct.ToString());
    }
}