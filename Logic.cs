using Reddit;
using Reddit.Controllers;
using Things = Reddit.Things;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace GRRAbstimmung
{
    internal class Logic
    {
        public static Reddit.RedditClient MyRC;

        public static int DELAY = 11000;

        // reading into the first three, seeing if Karma is enough in one of them, then adding to Combined List.
        // loop through combnined List and then write to new post
        public static List<TopLevelComment> ListTopLevelCommentsA = new List<TopLevelComment>();
        public static List<TopLevelComment> ListTopLevelCommentsB = new List<TopLevelComment>();
        public static List<TopLevelComment> ListTopLevelCommentsC = new List<TopLevelComment>();
        public static List<TopLevelComment> ListTopLevelCommentsCombined = new List<TopLevelComment>();

        public static Post MyPostWeCopyFrom;


        public static void Run()
        {
            // Initiate Logger
            Helper.Logger.Init();

            // Read Settings
            Helper.Logger.Log("Reading Settings...");
            Settings.ReadSettings();
            Helper.Logger.Log("Reading Settings...DONE");

            MyRC = new Reddit.RedditClient(appId: Options.REDDIT_CLIENT_ID, appSecret: Options.REDDIT_CLIENT_SECRET, refreshToken: Options.REDDIT_REFRESH_TOKEN);

            MyPostWeCopyFrom = GetPostFrom(Options.COPYCOMMENTSFROM);

            ListTopLevelCommentsA = GetTopLevel(MyPostWeCopyFrom, DELAY);
            ListTopLevelCommentsB = GetTopLevel(MyPostWeCopyFrom, DELAY);
            ListTopLevelCommentsC = GetTopLevel(MyPostWeCopyFrom, DELAY);

            // Loop through all TopLevels
            for (int i = 0; i <= ListTopLevelCommentsA.Count - 1; i++)
            {
                // create TopLevelComment
                new TopLevelComment(ListTopLevelCommentsA[i].TopLevelCommentContent, ref ListTopLevelCommentsCombined);

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

        }



        /// <summary>
        /// Gets a Post Object by its Permalink
        /// </summary>
        /// <param name="permalink"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        static public Post GetPostFrom(string permalink)
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
