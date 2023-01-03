using Reddit;
using Reddit.Controllers;
using Things = Reddit.Things;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace GRRAbstimmung
{
    internal class Logic
    {
        public static void Run()
        {
            Reddit.RedditClient MyRC = null;


            // reading into the first three, seeing if Karma is enough in one of them, then adding to Combined List.
            // loop through combnined List and then write to new post
            List<TopLevelComment> ListTopLevelCommentsA = new List<TopLevelComment>();
            List<TopLevelComment> ListTopLevelCommentsB = new List<TopLevelComment>();
            List<TopLevelComment> ListTopLevelCommentsC = new List<TopLevelComment>();
            List<TopLevelComment> ListTopLevelCommentsCombined = new List<TopLevelComment>();

            Post MyPostWeCopyFrom = null;
            Post MyPostWeCopyTo = null;


            // Initiate Logger
            Helper.Logger.Init();

            // Read Settings
            Helper.Logger.Log("Reading Settings...", true);
            Settings.ReadSettings();
            Helper.Logger.Log("Reading Settings...DONE", true);

            Helper.Logger.Log("Iniatiating Reddit Client...", true);
            try
            {
                MyRC = new Reddit.RedditClient(appId: Options.REDDIT_CLIENT_ID, appSecret: Options.REDDIT_CLIENT_SECRET, refreshToken: Options.REDDIT_REFRESH_TOKEN);
            }
            catch (Exception ex)
            {
                ErrorOut("Error while creating Reddit Client.\n" + ex.Message);
            }
            Helper.Logger.Log("Iniatiating Reddit Client...DONE", true);

            Helper.Logger.Log("Iniatiating PostWeCopyFrom...", true);
            try
            {
                MyPostWeCopyFrom = GetPostFrom(Options.COPYCOMMENTSFROM, MyRC);
            }
            catch (Exception ex)
            {
                ErrorOut("Error while creating MyPostWeCopyFrom.\n" + ex.Message);
            }
            Helper.Logger.Log("Iniatiating PostWeCopyFrom...DONE", true);

            Helper.Logger.Log("Iniatiating PostWeCopyTo...", true);
            try
            {
                MyPostWeCopyTo = GetPostFrom(Options.COPYCOMMENTSTO, MyRC);
            }
            catch (Exception ex)
            {
                ErrorOut("Error while creating MyPostWeCopyTo.\n" + ex.Message);
            }
            Helper.Logger.Log("Iniatiating PostWeCopyTo...DONE", true);

            Helper.Logger.Log("Gathering all Comments 1/3...this will take a few seconds...", true);
            ListTopLevelCommentsA = GetTopLevel(MyPostWeCopyFrom, Options.DELAY);
            Helper.Logger.Log("Gathering all Comments 1/3...this will take a few seconds...DONE", true);

            Helper.Logger.Log("Gathering all Comments 2/3...this will take a few seconds...", true);
            ListTopLevelCommentsB = GetTopLevel(MyPostWeCopyFrom, Options.DELAY);
            Helper.Logger.Log("Gathering all Comments 2/3...this will take a few seconds...DONE", true);

            Helper.Logger.Log("Gathering all Comments 3/3...this will take a few seconds...", true);
            ListTopLevelCommentsC = GetTopLevel(MyPostWeCopyFrom, Options.DELAY);
            Helper.Logger.Log("Gathering all Comments 3/3...this will take a few seconds...DONE", true);

            Helper.Logger.Log("Calculating Combined List...", true);
            // Loop through all TopLevels
            for (int i = 0; i <= ListTopLevelCommentsA.Count - 1; i++)
            {
                // create TopLevelComment
                new TopLevelComment(ListTopLevelCommentsA[i].Content, ref ListTopLevelCommentsCombined);

                // Loop through all of the Subcomments
                for (int j = 0; j <= ListTopLevelCommentsA[i].ListOfSubs.Count - 1; j++)
                {
                    if ((ListTopLevelCommentsA[i].ListOfSubs[j].Karma >= Options.KARMAREQUIRED) &&
                        (ListTopLevelCommentsB[i].ListOfSubs[j].Karma >= Options.KARMAREQUIRED) &&
                        (ListTopLevelCommentsC[i].ListOfSubs[j].Karma >= Options.KARMAREQUIRED))

                    {
                        // add Reply to ListTopLevelCombined with Content, Author, Karma
                        ListTopLevelCommentsCombined[ListTopLevelCommentsCombined.Count - 1].AddReply(ListTopLevelCommentsA[i].ListOfSubs[j].Content, ListTopLevelCommentsA[i].ListOfSubs[j].Author, ListTopLevelCommentsA[i].ListOfSubs[j].Karma);
                    }
                }
            }
            Helper.Logger.Log("Calculating Combined List...DONE", true);

            Helper.Logger.Log("Running PostProduction...", true);
            ListTopLevelCommentsCombined = PostProduction(ListTopLevelCommentsCombined);
            Helper.Logger.Log("Running PostProduction...DONE", true);

            Helper.Logger.Log("Copying Comments...this will take a while...");
            CopyComments(ListTopLevelCommentsCombined, MyPostWeCopyTo, Options.DELAY);
            Helper.Logger.Log("Copying Comments...this will take a while...DONE");
            Console.Clear();
            Console.WriteLine("DONE");
            Environment.Exit(0);
        }



        public static void ErrorOut(string pMessage)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERROR ! ! !");
            Console.WriteLine("ERROR ! ! !");
            Console.WriteLine("ERROR ! ! !");
            Console.WriteLine(pMessage);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Program will try to log and then exit after pressing [ENTER]");
            string asdf = Console.ReadLine();
            Helper.Logger.Log(pMessage);
            Environment.Exit(0);
        }

        public static void CopyComments(List<TopLevelComment> TopLevelComments, Post MyNewPost, int DelayMS)
        {
            Stopwatch MyStopWatch = new Stopwatch();
            MyStopWatch.Start();

            int allCommentCount = 0;
            int allCommentCurrentCount = 0;
            foreach (TopLevelComment MTL in TopLevelComments)
            {
                allCommentCount++;
                foreach (SubLevelComment MSL in MTL.ListOfSubs)
                {
                    allCommentCount++;
                }
            }


            int manualcount = 0;



            for (int j = 0; j <= TopLevelComments.Count - 1; j++)
            {
                Console.Clear();
                Console.WriteLine("This might take a while, it sleeps " + Options.DELAY + " ms between posting Comments.");
                Console.WriteLine("just wait sunshine");
                manualcount++;
                Helper.Logger.Log("On TopLevelComment: "+ manualcount + " from " + TopLevelComments.Count + " (" + TopLevelComments[j].Content + ").", true);

                Comment tmp = null;
                try
                {
                    tmp = MyNewPost.Reply(TopLevelComments[j].Content);
                }
                catch
                {
                    ErrorFuckedUs(TopLevelComments);
                }
                System.Threading.Thread.Sleep(DelayMS);
                allCommentCurrentCount++;



                for (int i = 0; i <= TopLevelComments[j].ListOfSubs.Count - 1; i++)
                {
                    allCommentCurrentCount++;


                    Helper.Logger.Log("--- On SubLevelComment: " + i + 1 + " from " + TopLevelComments[j].ListOfSubs.Count + " (" + TopLevelComments[j].ListOfSubs[i].Content + ").", true);
                    Helper.Logger.Log("--- --- Comment " + allCommentCurrentCount + " from " + allCommentCount + " overall.", true);
                    Helper.Logger.Log("--- --- Took " + MyStopWatch.Elapsed.Minutes + " Min and " + MyStopWatch.Elapsed.Seconds + "." + MyStopWatch.Elapsed.Milliseconds + " Secs to get here.", true);

                    try
                    {
                        tmp.Reply(TopLevelComments[j].ListOfSubs[i].Content);
                    }
                    catch
                    {
                        ErrorFuckedUs(TopLevelComments);
                    }
                    System.Threading.Thread.Sleep(DelayMS);

                    TopLevelComments[j].ListOfSubs[i].WrittenToReddit = true;


                }

                TopLevelComments[j].WrittenToReddit = true;

            }
        }



        public static void ErrorFuckedUs(List<TopLevelComment> TopLevelComments)
        {
            List<string> output = new List<string>();
            foreach (TopLevelComment MTL in TopLevelComments)
            {
                output.Add(MTL.WrittenToReddit.ToString() + "|" + MTL.Content);
                foreach (SubLevelComment MSL in MTL.ListOfSubs)
                {
                    output.Add(MTL.WrittenToReddit.ToString() + "|-----|" + MSL.Content);
                }
            }
            Helper.FileHandling.WriteStringToFileOverwrite(Options.ErrorFile, output.ToArray());
        }




        public static List<TopLevelComment> PostProduction(List<TopLevelComment> pInput)
        {
            List<TopLevelComment> myBackup = pInput;

            List<TopLevelComment> myRtrn = new List<TopLevelComment>();

            for (int i = 0; i <= myBackup.Count - 1; i++)
            {
                // replace EINREICHUNGEN with ABSTIMMUNG
                myBackup[i].Content = myBackup[i].Content.Replace("EINREICHUNG", "ABSTIMMUNG");

                // Trim the last three dots
                myBackup[i].Content.TrimEnd('.');
                myBackup[i].Content.TrimEnd('.');
                myBackup[i].Content.TrimEnd('.');

                // if its not the top level comment by henneye with information
                if (!myBackup[i].Content.ToLower().Contains("Community-Einreichungen".ToLower()))
                {
                    new TopLevelComment(myBackup[i].Content, ref myRtrn);
                    for (int j = 0; j <= myBackup[i].ListOfSubs.Count - 1; j++)
                    {
                        if (!myBackup[i].ListOfSubs[j].Content.ToLower().Contains("Shirin David Album Bitches brauchen Rap"))
                        {
                            myRtrn[myRtrn.Count - 1].AddReply(myBackup[i].ListOfSubs[j].Content, myBackup[i].ListOfSubs[j].Author, myBackup[i].ListOfSubs[j].Karma);
                        }
                    }
                }
            }

            return myRtrn;
        }



        /// <summary>
        /// Gets a Post Object by its Permalink
        /// </summary>
        /// <param name="permalink"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        static public Post GetPostFrom(string permalink, RedditClient MyRC)
        {
            // Get the ID from the permalink, then preface it with "t3_" to convert it to a Reddit fullname.  --Kris
            Match match = Regex.Match(permalink, @"\/comments\/([a-z0-9]+)\/");

            string postFullname = "t3_" + (match != null && match.Groups != null && match.Groups.Count >= 2
                ? match.Groups[1].Value
                : "");
            if (postFullname.Equals("t3_"))
            {
                throw new Exception("Unable to extract ID from permalink.");
            }

            return MyRC.Post(postFullname).About();
        }


        /// <summary>
        /// Method to get List of TopLevelComments (and their subcomments) for a Post
        /// </summary>
        /// <param name="Delay"></param>
        /// <returns></returns>
        public static List<TopLevelComment> GetTopLevel(Post myPost, int Delay)
        {
            List<TopLevelComment> ListOfTopLevels = new List<TopLevelComment>();

            ListOfTopLevels = IterateComments(myPost.Comments.GetNew(limit: 500), 0, myPost);

            return ListOfTopLevels;
        }


        /// <summary>
        /// Creates lists etc. and then calls IterateCommentsRecursive
        /// </summary>
        /// <param name="comments"></param>
        /// <param name="depth"></param>
        /// <param name="MyPost"></param>
        /// <returns></returns>
        private static List<TopLevelComment> IterateComments(IList<Comment> comments, int depth, Post MyPost)
        {
            List<TopLevelComment> TLC_List = new List<TopLevelComment>();
            HashSet<string> MyHS = new HashSet<string>();
            IterateCommentsRecursive(comments, depth, MyPost, ref MyHS, ref TLC_List);
            return TLC_List;
        }


        /// <summary>
        /// Loops through Comments, processes, gets more comments recursive
        /// </summary>
        /// <param name="comments"></param>
        /// <param name="depth"></param>
        /// <param name="MyPost"></param>
        /// <param name="MyHS"></param>
        /// <param name="TLC_List"></param>
        private static void IterateCommentsRecursive(IList<Comment> comments, int depth, Post MyPost, ref HashSet<string> MyHS, ref List<TopLevelComment> TLC_List)
        {
            foreach (Comment comment in comments)
            {
                ProcessComment(comment, depth, ref MyHS, ref TLC_List);
                IterateCommentsRecursive(comment.Replies, (depth + 1), MyPost, ref MyHS, ref TLC_List);
                IterateCommentsRecursive(GetMoreChildren(comment, MyPost, ref MyHS), depth, MyPost, ref MyHS, ref TLC_List);
            }
        }

        /// <summary>
        /// Method to get more Children
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="MyPost"></param>
        /// <param name="MyHS"></param>
        /// <returns></returns>
        private static IList<Comment> GetMoreChildren(Comment comment, Post MyPost, ref HashSet<string> MyHS)
        {
            List<Comment> res = new List<Comment>();
            if (comment.More == null)
            {
                return res;
            }

            foreach (Things.More more in comment.More)
            {
                foreach (string id in more.Children)
                {
                    // Only add if that comment hasnt been added yet
                    if (!MyHS.Contains(id))
                    {
                        res.Add(MyPost.Comment("t1_" + id).About());
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// Processing of a found comment
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="depth"></param>
        /// <param name="HS"></param>
        /// <param name="TLC_List"></param>
        private static void ProcessComment(Comment comment, int depth, ref HashSet<string> HS, ref List<TopLevelComment> TLC_List)
        {
            // exit when comment is removed
            if (comment.Removed)
            {
                return;
            }

            // exit when comment is null, no author or no content
            if (comment == null || string.IsNullOrWhiteSpace(comment.Author) || string.IsNullOrWhiteSpace(comment.Body))
            {
                return;
            }

            // if hash list doesnt already contain it, add to it
            if (!HS.Contains(comment.Id))
            {
                HS.Add(comment.Id);
            }


            // if depth less than 2
            if (comment.Depth < 2)
            {
                // if depth is zero (toplevelcomment)
                if (depth.Equals(0))
                {
                    new TopLevelComment(comment.Body, ref TLC_List);
                }
                // else its a reponse to a toplevelcomment
                else
                {
                    // add it as reply to the latest top level comment
                    TLC_List[TLC_List.Count - 1].AddReply(comment.Body, comment.Author, comment.UpVotes);
                }

                //Console.WriteLine("[" + comment.Author + "] " + comment.Body);
            }
        }

    }
}
