﻿using WebApplicationBPR2.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace WebApplicationBPR2.Data
{
        // we need this context class to interface with our physical database
    public class DataContext : IdentityDbContext<IdentityUser>
    {
        // we meed to inject the settings from appsettings.json, so we need to inject DbContextOptions of type DataContext in our DataContext (we do that through a constructor)
        public DataContext(DbContextOptions<DataContext> dbContextOptions) : base(dbContextOptions)
        {
        }
        // properties created for entities that we want to query for 
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; } // to set cart orders to database
    }
}
