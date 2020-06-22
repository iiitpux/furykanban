using System;
using System.Collections.Generic;
using System.Text;

namespace FuryKanban.DataLayer.Dto
{
	public class HistoryDto
	{
		public int Id { set; get; }
		public int UserId { set; get; }
		public UserDto User { set; get; }
		public string Title { set; get; }
		public string Body { set; get; }
	}
}
