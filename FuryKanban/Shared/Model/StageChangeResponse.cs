using System;
using System.Collections.Generic;
using System.Text;

namespace FuryKanban.Shared.Model
{
	public class StageChangeResponse
	{
		public bool HasError { set; get; } = false;
		public string ErrorMessage { set; get; }
		public List<AppState.Stage> Stages { set; get; }
	}
}
