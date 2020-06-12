using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FuryKanban.DataLayer.Dto
{
    public class ProjectDto
    {
        [Key]
        public int Id { set; get; }
        
        public string Title { set; get; }
        
        public virtual List<StageDto> Stages { set; get; }
        
        public bool IsSelected { set; get; }
    }
}
