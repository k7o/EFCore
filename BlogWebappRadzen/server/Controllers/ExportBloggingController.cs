using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogWebappRadzen.Data;

namespace BlogWebappRadzen
{
    public partial class ExportBloggingController : ExportController
    {
        private readonly BloggingContext context;

        public ExportBloggingController(BloggingContext context)
        {
            this.context = context;
        }
        [HttpGet("/export/Blogging/blogs/csv")]
        [HttpGet("/export/Blogging/blogs/csv(fileName='{fileName}')")]
        public FileStreamResult ExportBlogsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(context.Blogs, Request.Query), fileName);
        }

        [HttpGet("/export/Blogging/blogs/excel")]
        [HttpGet("/export/Blogging/blogs/excel(fileName='{fileName}')")]
        public FileStreamResult ExportBlogsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(context.Blogs, Request.Query), fileName);
        }
        [HttpGet("/export/Blogging/posts/csv")]
        [HttpGet("/export/Blogging/posts/csv(fileName='{fileName}')")]
        public FileStreamResult ExportPostsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(context.Posts, Request.Query), fileName);
        }

        [HttpGet("/export/Blogging/posts/excel")]
        [HttpGet("/export/Blogging/posts/excel(fileName='{fileName}')")]
        public FileStreamResult ExportPostsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(context.Posts, Request.Query), fileName);
        }
    }
}
