# EFCore Tests

## Setup

### Download a sqlserver container
https://hub.docker.com/_/microsoft-mssql-server

```powershell
docker pull mcr.microsoft.com/mssql/server:2019-latest
````
### Run it
```powershell
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=gN7quwVBof5BEMPpwQLU' -p 1433:1433 -v G:\Projects\Dotnet\EFCore:/sql -d mcr.microsoft.com/mssql/server:2019-latest
```

### Easy access to your db instance (with vscode)
https://marketplace.visualstudio.com/items?itemName=ms-mssql.mssql

use "localhost" not the docker container ip as db location

https://blog.sqlauthority.com/2019/03/06/sql-server-how-to-get-started-with-docker-containers-with-latest-sql-server/
https://blog.sqlauthority.com/2019/04/20/sql-server-docker-volume-and-persistent-storage/

## Get started

### Docs
https://docs.microsoft.com/en-us/ef/core/get-started/overview/install
https://docs.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli

### Steps
1. Create DL project
```powershell
dotnet new classlib -o BlogDL 
```
2. Add packages
```powershell
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design
```
3. Create model
```csharp
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
            => options.UseSqlServer(@"Server=localhost;Database=blogging;User Id=sa;Password=gN7quwVBof5BEMPpwQLU;");
    }

    [Comment("Blogs managed on the website")]
    public class Blog
    {
        public int BlogId { get; set; }
        [Display(Name = "Blog URL")]
        [Required]
        public string Url { get; set; }
        public List<Post> Posts { get; } = new List<Post>();
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
        public int BlogId { get; set; }
        [Display(Name = "Blog")]
        public Blog Blog { get; set; }
    }
}
```
4. Create database 
```sql
CREATE DATABASE blogging
```
5. Create initial migration
```powershell
dotnet ef migrations add InitialCreate
```
6. Update database
```powershell
dotnet ef database update
```

## Frontend
### Docs
https://docs.microsoft.com/en-us/aspnet/core/data/ef-rp/intro?view=aspnetcore-5.0&tabs=visual-studio-code
### Steps
1. Create WebApp
```powershell
dotnet new webapp -o BlogWebapp
```
2. Add reference to DL project
```powershell
dotnet add reference ..\BlogDL\BlogDL.csproj
```
3. Add packages
```powershell
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design -v 5.0.0-*
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design -v 5.0.0-*
dotnet add package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore -v 5.0.0-* 
```
4. Install dotnet-aspnet-codegenerator
```powershell
dotnet tool install --global dotnet-aspnet-codegenerator
```
5. Generate CRUD pages for Blog entity
```powershell
dotnet aspnet-codegenerator razorpage -m BlogDL.Blog -dc BlogDL.BloggingContext -udl -outDir Pages\Blogs
```
6. Generate CRUD pages for Post entity
```powershell
dotnet aspnet-codegenerator razorpage -m BlogDL.Post -dc BlogDL.BloggingContext -udl -outDir Pages\BlogPosts
```
7. Edit startup.cs to use DbContext
```csharp
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddDbContext<BloggingContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("BloggingContext")));
        }

```


https://docs.microsoft.com/en-us/aspnet/core/data/ef-rp/complex-data-model?view=aspnetcore-5.0&tabs=visual-studio

## Misc

### Intellisense issues VSCode
https://stackoverflow.com/questions/29975152/intellisense-not-automatically-working-vscode

1. Ctrl + Shift + p
2. Write "OmniSharp: Select Project" and press Enter.
3. Choose the solution workspace entry.