using System;
using System.Collections.Generic;
using System.Text;

namespace FuryKanban.Shared.Model
{
	public class BaseError
	{
		public bool HasError => !String.IsNullOrWhiteSpace(ErrorMessage);
		public string ErrorMessage { set; get; }
	}
}
