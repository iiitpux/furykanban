using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FuryKanban.DataLayer.Dto
{
    public class TokenDto
    {
        [Key]
        public string Code { set; get; }

        public System.DateTime CreatedDate { set; get; }
        
        public int UserId { set; get; }

        public virtual UserDto User { set; get; }
    }
}
