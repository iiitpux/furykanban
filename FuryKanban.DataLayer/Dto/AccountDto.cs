using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FuryKanban.DataLayer.Dto
{
	public class AccountDto
	{
		[Key]
		public int Id { set; get; }
	}
}
