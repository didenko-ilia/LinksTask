using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LinksTask.Models;

namespace LinksTask.Data
{
  public class LinkContext : DbContext
  {
    public LinkContext(DbContextOptions<LinkContext> options) : base(options) { }
    
    public DbSet<Link> Links { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Link>().ToTable("links");
    }
  }
}
