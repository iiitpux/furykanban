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
		[Required]
		public string Password { set; get; }
		[CompareProperty(nameof(Password), ErrorMessage = "The Password didn't match.")]
		public string ConfirmPassword { set; get; }
	}
}
