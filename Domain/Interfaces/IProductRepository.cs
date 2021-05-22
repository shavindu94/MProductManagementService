﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IProductRepository:IGenericRepository<Product> 
    {
        List<Product> GetAllList();
    }
}
