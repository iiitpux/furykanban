using FuryKanban.Shared.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace FuryKanban.Shared.Interfaces
{
	public interface IAppStateResult
	{
		AppState AppState { set; get; }
	}
}
