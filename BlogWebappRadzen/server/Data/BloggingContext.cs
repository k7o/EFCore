using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;

using BlogWebappRadzen.Models.Blogging;

namespace BlogWebappRadzen.Data
{
  public partial class BloggingContext : Microsoft.EntityFrameworkCore.DbContext
  {
    public BloggingContext(DbContextOptions<BloggingContext> options):base(options)
    {
    }

    public BloggingContext()
    {
    }

    partial void OnModelBuilding(ModelBuilder builder);

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<BlogWebappRadzen.Models.Blogging.Post>()
              .HasOne(i => i.Blog)
              .WithMany(i => i.Posts)
              .HasForeignKey(i => i.BlogId)
              .HasPrincipalKey(i => i.BlogId);


        builder.Entity<BlogWebappRadzen.Models.Blogging.Blog>()
              .Property(p => p.BlogId)
              .HasPrecision(10, 0);

        builder.Entity<BlogWebappRadzen.Models.Blogging.Post>()
              .Property(p => p.PostId)
              .HasPrecision(10, 0);

        builder.Entity<BlogWebappRadzen.Models.Blogging.Post>()
              .Property(p => p.BlogId)
              .HasPrecision(10, 0);
        this.OnModelBuilding(builder);
    }


    public DbSet<BlogWebappRadzen.Models.Blogging.Blog> Blogs
    {
      get;
      set;
    }

    public DbSet<BlogWebappRadzen.Models.Blogging.Post> Posts
    {
      get;
      set;
    }
  }
}
