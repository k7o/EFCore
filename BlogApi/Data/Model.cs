using BlogShared;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Data
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
            => options.UseSqlServer(@"Server=localhost;Database=blogging;User Id=sa;Password=gN7quwVBof5BEMPpwQLU;");
    }
}