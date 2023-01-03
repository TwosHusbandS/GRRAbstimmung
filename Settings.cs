using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRRAbstimmung
{
    internal class Settings
    {
        public static void ReadSettings()
        {
            string configFileContent = Helper.FileHandling.ReadContentOfFile(Options.Configfile);
            string bodyFileContent = Helper.FileHandling.ReadContentOfFile(Options.Bodyfile);

            Options.REDDIT_CLIENT_ID = Helper.FileHandling.GetXMLTagContent(configFileContent, "REDDIT_CLIENT_ID");
            Options.REDDIT_CLIENT_SECRET = Helper.FileHandling.GetXMLTagContent(configFileContent, "REDDIT_CLIENT_SECRET");
            Options.REDDIT_ACCESS_TOKEN = Helper.FileHandling.GetXMLTagContent(configFileContent, "REDDIT_ACCESS_TOKEN");
            Options.REDDIT_REFRESH_TOKEN = Helper.FileHandling.GetXMLTagContent(configFileContent, "REDDIT_REFRESH_TOKEN");
            Options.COPYCOMMENTSFROM = Helper.FileHandling.GetXMLTagContent(configFileContent, "COPYCOMMENTSFROM");
            Options.COPYCOMMENTSTO = Helper.FileHandling.GetXMLTagContent(configFileContent, "COPYCOMMENTSTO");
            string tmp = Helper.FileHandling.GetXMLTagContent(configFileContent, "KARMAREQUIRED");
            try
            {
                Options.KARMAREQUIRED = Int32.Parse(tmp);
            }
            catch (Exception ex)
            {
                Options.KARMAREQUIRED = 0;
            }
            string tmpp = Helper.FileHandling.GetXMLTagContent(configFileContent, "DELAY");
            try
            {
                Options.DELAY = Int32.Parse(tmpp);
            }
            catch (Exception ex)
            {
                Options.DELAY = 11000;
            }


            string logstring = "---- read all settings:";
            logstring += " Options.REDDIT_CLIENT_ID='" + Options.REDDIT_CLIENT_ID + "'";
            logstring += " Options.REDDIT_CLIENT_SECRET='" + Options.REDDIT_CLIENT_SECRET + "'";
            logstring += " Options.REDDIT_ACCESS_TOKEN='" + Options.REDDIT_ACCESS_TOKEN + "'";
            logstring += " Options.REDDIT_REFRESH_TOKEN='" + Options.REDDIT_REFRESH_TOKEN + "'";
            logstring += " Options.COPYCOMMENTSFROM='" + Options.COPYCOMMENTSFROM + "'";
            logstring += " Options.COPYCOMMENTSTO='" + Options.COPYCOMMENTSTO + "'";
            logstring += " Options.KARMAREQUIRED='" + Options.KARMAREQUIRED + "'";
            Helper.Logger.Log(logstring);

        }
    }
}
