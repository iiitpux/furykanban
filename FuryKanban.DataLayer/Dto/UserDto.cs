using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FuryKanban.DataLayer.Dto
{
    public class UserDto
    {
        [Key]
        public int Id { set; get; }

        [Required]
        public string Login { set; get; }
        
        [Required]
        public string Password { set; get; }
        
        public string Salt { set; get; }
        
        public bool Active { set; get; }
        
        public DateTime CreateDate { set; get; }
    }
}
