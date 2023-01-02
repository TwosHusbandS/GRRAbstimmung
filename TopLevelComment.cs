using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRRAbstimmung
{
    internal class TopLevelComment
    {
        public string TopLevelCommentContent = "";
        public List<SubLevelComment> ListOfSubs = new List<SubLevelComment>();
        public bool WrittenToReddit;

		public TopLevelComment(string _TopLevelComment, ref List<TopLevelComment> TLC_List)
		{
			TopLevelCommentContent = _TopLevelComment;
			this.WrittenToReddit = false;

			TLC_List.Add(this);
		}

		public void AddReply(string tmp, string asgg, int ase)
		{
			ListOfSubs.Add(new SubLevelComment(tmp, asgg, ase));
		}
	}
}
