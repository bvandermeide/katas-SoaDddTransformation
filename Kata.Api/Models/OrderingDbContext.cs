using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Kata.Api.Models
{
  public class OrderingDbContext : DbContext
  {
    public OrderingDbContext(DbContextOptions<OrderingDbContext> options)
        : base(options)
    { }

    public DbSet<Customer> Customers { get; set; }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> Items { get; set; }
    public DbSet<Address> Addresses { get; set; }

    public DbSet<Product> Products { get; set; }
  }
}