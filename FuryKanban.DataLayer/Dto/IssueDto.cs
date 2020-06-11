using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FuryKanban.DataLayer.Dto
{
    public class IssueDto
    {
        [Key]
        public int Id { set; get; }
        
        public string Title { set; get; }
        
        public string Description { set; get; }
        
        public DateTime AssignedDateTime { set; get; }
        
        public DateTime CreatedDateTime { set; get; }

        public int AssignedUserId { set; get; }

        public UserDto AssignedUser { set; get; }

        public int CreatedUserId { set; get; }

        public UserDto CreatedUser { set; get; }
        
        public int StageId { set; get; }

        public StageDto Stage { set; get; }

        public int Order { set; get; }
    }
}
