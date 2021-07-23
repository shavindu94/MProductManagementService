using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Dto
{
    public class DTOPagination
    {
        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public int TotalNumber { get; set; }

        public string SortString { get; set; }

        public string SearchString { get; set; }

        public int NumberOfpages
        {
            get { return (int)Math.Ceiling(TotalNumber / (double)PageSize); }
        }

        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < NumberOfpages;

        public object Data { get; set; }

        /// <summary>
        ///Use Later 2
        /// </summary>
        public DTOPagination()
        {

        }

  


    }
}
