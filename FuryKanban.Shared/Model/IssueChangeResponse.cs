using FuryKanban.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FuryKanban.Shared.Model
{
	public class IssueChangeResponse : BaseError, IAppStateResult, IErrorResult
	{
		public AppState AppState { set; get; }
	}
}
