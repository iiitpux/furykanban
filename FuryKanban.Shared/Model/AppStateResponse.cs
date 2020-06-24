using FuryKanban.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FuryKanban.Shared.Model
{
	public class AppStateResponse : BaseError,  IErrorResult, IAppStateResult
	{
		public AppState AppState { set; get; }
	}
}
