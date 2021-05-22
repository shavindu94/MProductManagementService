using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class Product:BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [ForeignKey("ProductType")]
        [Required]
        public Guid ProductTypeId  { get; set; }

        public ProductType ProductType { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}
