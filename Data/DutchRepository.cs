using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchRepository : IDutchRepository
    {
        private readonly DutchContext ctx;
        private readonly ILogger<DutchRepository> logger;

        public DutchRepository(DutchContext ctx,ILogger<DutchRepository> logger)
        {
            this.ctx = ctx;
            this.logger = logger;
        }

        public IEnumerable<Product> GetAllProducts()
        {

            this.logger.LogInformation("GetAllProducts was called");
            return this.ctx.Products
                       .OrderBy(p => p.Title)
                       .ToList();
        }

        public IEnumerable<Order> GetAllOrders(bool includeItems)
        {
            if (includeItems)
            {
                return this.ctx.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .ToList();
            }
            else
            {
                return this.ctx.Orders
                .ToList();
            }
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return this.ctx.Products
                       .Where(p => p.Category == category)
                       .ToList();
        }

        public bool SaveAll()
        {
           return  this.ctx.SaveChanges() > 0;
        }

        public Order GetOrderById(string username,int id)
        {
            return this.ctx.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o=>o.Id==id && o.User.UserName==username)
                .FirstOrDefault();
        }

        public void AddEntity(object model)
        {
            this.ctx.Add(model);
        }

        public IEnumerable<Order> GetAllOrdersByUser(string username, bool includeItems)
        {
            if (includeItems)
            {
                return this.ctx.Orders
                .Where(o=>o.User.UserName==username)
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .ToList();
            }
            else
            {
                return this.ctx.Orders
                    .Where(o => o.User.UserName == username)
                .ToList();
            }
        }
    }
}
