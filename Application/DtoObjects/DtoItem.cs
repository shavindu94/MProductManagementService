using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DtoObjects
{
    public class DtoItem
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string ImageUrl { get; set; }
        public string ItemDetail { get; set; }
        public Guid ProductId { get; set; }

    }
}


