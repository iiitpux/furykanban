using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FuryKanban.Shared.Model.Security
{
	public class LoginRequest
	{
		[Required]
		public string Login { set; get; }
		[Required]
		public string Password { set; get; }
	}
}
