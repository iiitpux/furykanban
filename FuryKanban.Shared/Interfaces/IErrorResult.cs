using System;
using System.Collections.Generic;
using System.Text;

namespace FuryKanban.Shared.Interfaces
{
	public interface IErrorResult
	{
		bool HasError { set; get; }
		string ErrorMessage { set; get; }
	}
}
