using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Image:BaseEntity
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public byte[] Picture { get; set; }
    }
}
