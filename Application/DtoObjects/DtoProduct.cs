using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DtoObjects
{
    public class DtoProduct
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public Guid ProductTypeId { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<DtoItem> Items { get; set; }
    }
}
