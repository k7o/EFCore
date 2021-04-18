using System;
using System.Net;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNet.OData.Query;



namespace BlogWebappRadzen.Controllers.Blogging
{
  using Models;
  using Data;
  using Models.Blogging;

  [ODataRoutePrefix("odata/Blogging/Blogs")]
  [Route("mvc/odata/Blogging/Blogs")]
  public partial class BlogsController : ODataController
  {
    private Data.BloggingContext context;

    public BlogsController(Data.BloggingContext context)
    {
      this.context = context;
    }
    // GET /odata/Blogging/Blogs
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    [HttpGet]
    public IEnumerable<Models.Blogging.Blog> GetBlogs()
    {
      var items = this.context.Blogs.AsQueryable<Models.Blogging.Blog>();
      this.OnBlogsRead(ref items);

      return items;
    }

    partial void OnBlogsRead(ref IQueryable<Models.Blogging.Blog> items);

    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    [HttpGet("{BlogId}")]
    public SingleResult<Blog> GetBlog(int key)
    {
        var items = this.context.Blogs.Where(i=>i.BlogId == key);
        return SingleResult.Create(items);
    }
    partial void OnBlogDeleted(Models.Blogging.Blog item);
    partial void OnAfterBlogDeleted(Models.Blogging.Blog item);

    [HttpDelete("{BlogId}")]
    public IActionResult DeleteBlog(int key)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var item = this.context.Blogs
                .Where(i => i.BlogId == key)
                .Include(i => i.Posts)
                .FirstOrDefault();

            if (item == null)
            {
                return BadRequest();
            }

            this.OnBlogDeleted(item);
            this.context.Blogs.Remove(item);
            this.context.SaveChanges();
            this.OnAfterBlogDeleted(item);

            return new NoContentResult();
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }

    partial void OnBlogUpdated(Models.Blogging.Blog item);
    partial void OnAfterBlogUpdated(Models.Blogging.Blog item);

    [HttpPut("{BlogId}")]
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    public IActionResult PutBlog(int key, [FromBody]Models.Blogging.Blog newItem)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (newItem == null || (newItem.BlogId != key))
            {
                return BadRequest();
            }

            this.OnBlogUpdated(newItem);
            this.context.Blogs.Update(newItem);
            this.context.SaveChanges();

            var itemToReturn = this.context.Blogs.Where(i => i.BlogId == key);
            this.OnAfterBlogUpdated(newItem);
            return new ObjectResult(SingleResult.Create(itemToReturn));
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }

    [HttpPatch("{BlogId}")]
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    public IActionResult PatchBlog(int key, [FromBody]Delta<Models.Blogging.Blog> patch)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = this.context.Blogs.Where(i => i.BlogId == key).FirstOrDefault();

            if (item == null)
            {
                return BadRequest();
            }

            patch.Patch(item);

            this.OnBlogUpdated(item);
            this.context.Blogs.Update(item);
            this.context.SaveChanges();

            var itemToReturn = this.context.Blogs.Where(i => i.BlogId == key);
            return new ObjectResult(SingleResult.Create(itemToReturn));
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }

    partial void OnBlogCreated(Models.Blogging.Blog item);
    partial void OnAfterBlogCreated(Models.Blogging.Blog item);

    [HttpPost]
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    public IActionResult Post([FromBody] Models.Blogging.Blog item)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (item == null)
            {
                return BadRequest();
            }

            this.OnBlogCreated(item);
            this.context.Blogs.Add(item);
            this.context.SaveChanges();

            return Created($"odata/Blogging/Blogs/{item.BlogId}", item);
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }
  }
}
