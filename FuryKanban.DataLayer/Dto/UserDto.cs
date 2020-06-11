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

        [DisplayName("Логин")]
        [Required(ErrorMessage = "Логин не должен быть пустым")]
        public string Login { set; get; }
        
        [DisplayName("Пароль")]
        [Required(ErrorMessage = "Пароль не должен быть пустым")]
        
        public string Password { set; get; }
        
        public string Salt { set; get; }
        
        public bool Active { set; get; }
        
        public DateTime CreateDate { set; get; }
        
        public int AccountId { set; get; }

        public AccountDto Account { set; get; }
    }
}
