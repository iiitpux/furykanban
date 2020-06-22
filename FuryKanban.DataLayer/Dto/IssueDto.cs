using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace FuryKanban.DataLayer.Dto
{
    public class IssueDto
    {
        [Key]
        public int Id { set; get; }
        
        public string Title { set; get; }

        public string Body { set; get; }
        
        public DateTime CreatedDateTime { set; get; }

        public int UserId { set; get; }

        public UserDto User { set; get; }
        
        public int StageId { set; get; }

        public StageDto Stage { set; get; }

        public int? NextIssueId { set; get; }
        
        public IssueDto NextIssue { set; get; }
    }
}
