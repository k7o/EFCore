using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApi.Data;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BlogApi
{
    public class BlogService : BlogProto.BlogProtoBase
    {
        private readonly ILogger<BlogService> logger;
        private readonly BloggingContext bloggingContext;

        public BlogService(ILogger<BlogService> logger, BloggingContext bloggingContext)
        {
            this.logger = logger;
            this.bloggingContext = bloggingContext;
        }

        public override async Task<Blogs> GetBlogs(Empty request, ServerCallContext context)
        {
            var query = await bloggingContext.Blogs
                    .Include(i => i.Posts)
                    .ToListAsync();
            
            var blogs = new Blogs();
            blogs.BlogsData.AddRange(query);
            
            return blogs;
        }
    }
}
