using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRRAbstimmung.Helper
{
    internal class Logger
    {
        // We should probably use a logging libary / framework now that I think about it...whatevs
        // Actually implementing this probably took less time than googling "Logging class c#", and we have more control over it

        private static Mutex mut = new Mutex();






        /// <summary>
        /// Init Function which gets called once at the start.
        /// </summary>
        public static void Init()
        {
            // Since the createFile Method will override an existing file
            if (!FileHandling.doesFileExist(Options.Logfile))
            {
                FileHandling.createFile(Options.Logfile);
            }


            string MyCreationDate = FileHandling.GetCreationDate(Process.GetCurrentProcess().MainModule.FileName).ToString("yyyy-MM-ddTHH:mm:ss");

            Logger.Log("-");
            Logger.Log("-");
            Logger.Log("-");
            Logger.Log(" === Logger thingy started === ");
            Logger.Log("    I was created (non UTC) at: '" + MyCreationDate + "'");
            Logger.Log("    Time Now: '" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "'");
            Logger.Log("    Time Now UTC: '" + DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss") + "'");
        }

        /// <summary>
        /// Main Method of Logging.cs which is called to log stuff.
        /// </summary>
        /// <param name="pLogMessage"></param>
        public static void Log(string pLogMessage, bool WriteToConsole = false)
        {
            if (WriteToConsole)
            {
                Console.WriteLine(pLogMessage);
            }

            try
            {

                mut.WaitOne();

                string LogMessage = "[" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "] - " + pLogMessage;

                FileHandling.AddToLog(Options.Logfile, LogMessage);

                mut.ReleaseMutex();
            }
            catch { } 
        }

    } // End of Class
} // End of NameSpace


