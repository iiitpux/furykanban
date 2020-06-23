using FuryKanban.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FuryKanban.Shared.Model
{
	public class AppStateResponse : IErrorResult, IAppStateResult
	{
		public bool HasError { set; get; }
		public string ErrorMessage { set; get; }
		public AppState AppState { set; get; }
	}
}
