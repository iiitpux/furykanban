using FuryKanban.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FuryKanban.Shared.Model
{
	public class IssueChangeResponse : IAppStateResult, IErrorResult
	{
		public bool HasError { set; get; } = false;
		public string ErrorMessage { set; get; }
		public AppState AppState { set; get; }
	}
}
