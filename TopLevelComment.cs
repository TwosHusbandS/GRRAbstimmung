using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRRAbstimmung
{
    internal class TopLevelComment
    {
        public string Content = "";
        public List<SubLevelComment> ListOfSubs = new List<SubLevelComment>();
		public bool WrittenToReddit;

		public TopLevelComment(string _TopLevelComment, ref List<TopLevelComment> TLC_List)
		{
			Content = _TopLevelComment;
			WrittenToReddit = false;

			TLC_List.Add(this);
		}

		public void AddReply(string pContent, string pAuthor, int pKarma)
		{
			ListOfSubs.Add(new SubLevelComment(pContent, pAuthor, pKarma));
		}
	}
}
