using System;
using Онлайн_Ресторан.Models;
using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;
using Онлайн_Ресторан.Models;
using Онлайн_Ресторан.Models;

public class MyBaseContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Kitchen> Kitchens { get; set; }
    public DbSet<Korzina> Korzinas { get; set; }
    public DbSet<Check> Checks { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Prodano> Prodanos { get; set; }

    public MyBaseContext(DbContextOptions<MyBaseContext> options)
           : base(options)
    {
        Database.EnsureCreated();
    }
}
