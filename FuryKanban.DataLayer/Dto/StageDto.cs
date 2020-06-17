using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FuryKanban.DataLayer.Dto
{
    public class StageDto
    {
        [Key]
        public int Id { set; get; }
        
        public string Title { set; get; }

        public int Order { set; get; }

        public int UserId { set; get; }
        
        public UserDto User { set; get; }

        public virtual List<IssueDto> Issues { set; get; }
    }
}
