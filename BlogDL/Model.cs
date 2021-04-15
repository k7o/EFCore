using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BlogDL
{
    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        public BloggingContext()
        {
        }

        public BloggingContext(DbContextOptions<BloggingContext> options) 
            : base(options)
        {
        }

        // The following configures EF to create a Sqlite database file as `C:\blogging.db`.
        // For Mac or Linux, change this to `/tmp/blogging.db` or any other absolute path.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseLazyLoadingProxies()
                      .UseSqlServer(@"Server=localhost;Database=blogging;User Id=sa;Password=gN7quwVBof5BEMPpwQLU;");
    }

    [Comment("Blogs managed on the website")]
    public class Blog
    {
        public int BlogId { get; set; }
        [Display(Name = "Blog URL")]
        [Required]
        public string Url { get; set; }
        public virtual List<Post> Posts { get; } = new List<Post>();
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
        public int BlogId { get; set; }
        [Display(Name = "Blog")]
        public virtual Blog Blog { get; set; }
    }
}