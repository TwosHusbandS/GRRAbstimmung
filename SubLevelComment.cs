using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRRAbstimmung
{
    internal class SubLevelComment
    {
        public int Karma;
        public string Author;
        public string Content;
        public bool WrittenToReddit;

        public SubLevelComment(string pContent, string pAuthor, int pKarma)
        {
            this.Karma = pKarma;
            this.Author = pAuthor;
            this.WrittenToReddit = false;
            this.Content = pContent;
        }
    }
}
