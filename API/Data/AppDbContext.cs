using System;
using Microsoft.EntityFrameworkCore;
using API.Entities;
namespace API.Data;


public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<AppUser> Users { get; set; }
}
