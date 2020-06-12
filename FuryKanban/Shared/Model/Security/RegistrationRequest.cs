using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text;

namespace FuryKanban.Shared.Model.Security
{
	public class RegistrationRequest
	{
		[Required]
		public string Login { set; get; }
		public string Password { set; get; }
		public string ConfirmPassword { set; get; }
	}
}
