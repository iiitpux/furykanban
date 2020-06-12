using System;
using System.Collections.Generic;
using System.Text;

namespace FuryKanban.Shared.Model.Security
{
	public class LoginRequest
	{
		public string Login { set; get; }
		public string Password { set; get; }
	}
}
