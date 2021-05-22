using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class Item :BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ItemId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public bool IsActive { get; set; }
        public string ImageUrl { get; set; }

        public string ItemDetail { get; set; }

        [ForeignKey("Product")]
        [Required]
        public Guid ProductId { get; set; }

        public Product Product { get; set; }
    }
}
