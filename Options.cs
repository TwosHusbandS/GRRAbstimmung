using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRRAbstimmung
{
    internal class Options
    {
        public static string REDDIT_CLIENT_ID = "";
        public static string REDDIT_CLIENT_SECRET = "";
        public static string REDDIT_ACCESS_TOKEN = "";
        public static string REDDIT_REFRESH_TOKEN = "";
        public static string COPYCOMMENTSFROM = "";
        public static string COPYCOMMENTSTO = "";
        public static int KARMAREQUIRED = 0;
        public static int DELAY = 11000;

        public static string Logfile
        {
            get
            {
                return Path.Combine(ProjectInstallationPath.TrimEnd(Path.DirectorySeparatorChar), @"logfile.log");
                //ProjectInstallationPath.TrimEnd('/') + @"/logfile.log";
            }
        }


        public static string ErrorFile
        {
            get
            {
                return Path.Combine(ProjectInstallationPath.TrimEnd(Path.DirectorySeparatorChar), @"Errorrrr.log");
            }
        }

        public static string Configfile
        {
            get
            {
                return Path.Combine(ProjectInstallationPath.TrimEnd(Path.DirectorySeparatorChar), @"config.ini");
                //ProjectInstallationPath.TrimEnd('/') + @"/config.ini";
            }
        }


        public static string Bodyfile
        {
            get
            {
                return Path.Combine(ProjectInstallationPath.TrimEnd(Path.DirectorySeparatorChar), @"body.txt");
                //ProjectInstallationPath.TrimEnd('/') + @"/body.txt";
            }
        }

        public static string ProjectInstallationPath
        {
            get
            {
                return System.AppContext.BaseDirectory;
                //return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                //return (Directory.GetParent(ProjectInstallationPathBinary).ToString());
            }
        }
    }
}
