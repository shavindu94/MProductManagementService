using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class BaseEntity
    {
        [Required]
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        [Required]
        public int CreatedUserId { get; set; }
        public int  ModifiedUserId { get; set; }
    }
}
