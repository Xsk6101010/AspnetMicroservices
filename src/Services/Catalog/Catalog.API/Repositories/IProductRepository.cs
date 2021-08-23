﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.API.Entities;

namespace Catalog.API.Repositories
{
    /// <summary>
    /// represent RepositoryPattern.
    /// </summary>
   public interface IProductRepository
   {
       Task<IEnumerable<Product>> GetProducts();
       Task<Product> GetProducts(string id);
       Task<IEnumerable<Product>> GetProductName(string name);
       Task<IEnumerable<Product>> GetProductByCategory(string categoryName);
       Task CreateProduct(Product product);
       Task<bool> UpdateProduct(Product product);
       Task<bool> DeleteProduct(string id);
    }
}
