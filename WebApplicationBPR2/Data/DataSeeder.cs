using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationBPR2.Data.Entities;

namespace WebApplicationBPR2.Data
{
    // used to seed data in our database
    public class DataSeeder
    {
        private readonly DataContext _dataContext;

        // we need to inject our DataContext so we can communicate with database
        public DataSeeder(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Seed()
        {
            _dataContext.Database.EnsureCreated(); // first we make sure that we have a database, so we avoid ominous exceptions

            if (!_dataContext.Products.Any()) // if there is no products (count = 0)
            {
                // add sample product
                var products = new List<Product>()
                {
                    new Product()
                    {
                        Category = new Category{CategoryName = "Ice Cream" },
                        Description = "A delicious profiterol",
                        Price = 13.99M,
                        Size = 400,
                        Title = "Profiterol De Jour",
                        PhotoId = "blackberry"
                    },
                    new Product
                    {
                        Category = new Category{CategoryName = "Cakes" },
                        Description = "A delicious cake",
                        Price = 19.99M,
                        Size = 700,
                        Title = "Othello Cake",
                        PhotoId = "grapes"
                    },
                    new Product
                    {
                        Category = new Category{CategoryName = "Tarts" },
                        Description = "A delicious tart",
                        Price = 3.99M,
                        Size = 100,
                        Title = "Dagmar tarte",
                        PhotoId = "pineapple"
                    }
            };
                
                _dataContext.Products.AddRange(products); // sends the product to the database
               

                // add sample order
                var order1 = new Order
                {
                    Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                           Product = products.First(),
                           Quantity = 3,
                           UnitPrice = products.First().Price
                        }
                    }
                };
                _dataContext.Add(order1); // sends the order to the database

                _dataContext.SaveChanges(); // save changes done to  database
            }
        }
    }
}
