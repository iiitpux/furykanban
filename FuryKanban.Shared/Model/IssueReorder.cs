using System;
using System.Collections.Generic;
using System.Text;

namespace FuryKanban.Shared.Model
{
	public class IssueReorder
	{
		public int Id { set; get; }
		public int TargetId { set; get; }
		public int NewStageId {set;get;}
	}
}
