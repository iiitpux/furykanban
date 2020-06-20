using System;
using System.Collections.Generic;
using System.Text;

namespace FuryKanban.DataLayer.Dto
{
	public class HistoryDto
	{
		public int Id { set; get; }
		public string Title { set; get; }
		public string Body { set; get; }
		public bool Committed { set; get; } = false;
	}
}
