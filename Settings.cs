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
            Options.SUBREDDIT = Helper.FileHandling.GetXMLTagContent(configFileContent, "SUBREDDIT");
            Options.FLAIR = Helper.FileHandling.GetXMLTagContent(configFileContent, "FLAIR");
            Options.REDDIT_POST_TITLE = Helper.FileHandling.GetXMLTagContent(configFileContent, "TITLE");
            Options.REDDIT_POST_BODY = bodyFileContent;
            if (Helper.FileHandling.GetXMLTagContent(configFileContent, "DISTINGUISH").ToUpper() == "FALSE")
            {
                Options.DISTINGUISH = false;
            }
            else
            {
                Options.DISTINGUISH = true;
            }
            if (Helper.FileHandling.GetXMLTagContent(configFileContent, "STICKY").ToUpper() == "TRUE")
            {
                Options.STICKY = true;
            }
            else
            {
                Options.STICKY = false;
            }
            Options.COPYCOMMENTSFROM = Helper.FileHandling.GetXMLTagContent(configFileContent, "COPYCOMMENTSFROM");
            string tmp = Helper.FileHandling.GetXMLTagContent(configFileContent, "KARMAREQUIRED");
            try
            {
                Options.KARMAREQUIRED = Int32.Parse(tmp);
            }
            catch (Exception ex)
            {
                Options.KARMAREQUIRED = 0;
            }



            string logstring = "---- read all settings:";
            logstring += " Options.REDDIT_CLIENT_ID='" + Options.REDDIT_CLIENT_ID + "'";
            logstring += " Options.REDDIT_CLIENT_SECRET='" + Options.REDDIT_CLIENT_SECRET + "'";
            logstring += " Options.REDDIT_ACCESS_TOKEN='" + Options.REDDIT_ACCESS_TOKEN + "'";
            logstring += " Options.REDDIT_REFRESH_TOKEN='" + Options.REDDIT_REFRESH_TOKEN + "'";
            logstring += " Options.SUBREDDIT='" + Options.SUBREDDIT + "'";
            logstring += " Options.REDDIT_POST_TITLE='" + Options.REDDIT_POST_TITLE + "'";
            logstring += " Options.REDDIT_POST_BODY.Length='" + Options.REDDIT_POST_BODY.Length + "'";
            logstring += " Options.DISTINGUISH='" + Options.DISTINGUISH + "'";
            logstring += " Options.STICKY='" + Options.STICKY + "'";
            logstring += " Options.COPYCOMMENTSFROM='" + Options.COPYCOMMENTSFROM + "'";
            logstring += " Options.KARMAREQUIRED='" + Options.KARMAREQUIRED + "'";
            Helper.Logger.Log(logstring);

        }
    }
}
