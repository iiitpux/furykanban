using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace FuryKanban.Shared.Model
{
	public class AppState
	{
		//todo linkedlist
		public List<Stage> Stages { set; get; }
		public List<Issue> Issues { set; get; }

		public class Stage
		{
			public int Id { set; get; }
			
			[Required]
			public string Title { set; get; }
			
			public int Order { set; get; }
			
			public List<Issue> Issues { set; get; }
		}

		public class Issue
		{
			public int Id { set; get; }
			
			public string Title { set; get; }
			
			public string Body { set; get; }
			
			public int Order { set; get; }
		}
	}
}
