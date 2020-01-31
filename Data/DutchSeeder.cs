using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext ctx;
        private readonly IHostingEnvironment hosting;
        private readonly UserManager<StoreUser> userManager;

        public DutchSeeder(DutchContext ctx, IHostingEnvironment hosting, UserManager<StoreUser>
            userManager)
        {
            this.ctx = ctx;
            this.hosting = hosting;
            this.userManager = userManager;
        }
        public async Task SeedAsync()
        {
            this.ctx.Database.EnsureCreated();


            StoreUser user = await this.userManager.FindByEmailAsync("shawn@dutchtreat.com");

            if (user==null)
            {
                user = new StoreUser()
                {
                    FirstName = "Shawn",
                    LastName = "Wildermuth",
                    Email = "shawn@dutchtreat.com",
                    UserName = "shawn@dutchtreat.com"
                };

                var result = await userManager.CreateAsync(user, "123T@marindo");
                if (result!=IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create a new user in seeder");
                }
            }

            if (!this.ctx.Products.Any())
            {
                var filepath = Path.Combine(this.hosting.ContentRootPath,"Data/art.json");
                //Need to create sample data
                var json = File.ReadAllText(filepath);
                var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
                this.ctx.Products.AddRange(products);

                var order = this.ctx.Orders.Where(o => o.Id == 1).FirstOrDefault();
                if (order!=null)
                {
                    order.User = user;
                    order.Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                           Product=products.First(),
                           Quantity=5,
                           UnitPrice=products.First().Price
                        }
                    };
                }

                this.ctx.SaveChanges();
            }
        }
    }
}
