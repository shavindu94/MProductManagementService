using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.EFCore.Repositories
{
    public class ItemRepository : GenericRepository<Item>, IItemRepository
    {
        public ItemRepository(ApplicationContext context) : base(context)
        {
        }

        public List<Item> GetItemList()
        {
           return _context.Items.Include(a => a.Product).ToList(); ;
        }
    }
}
