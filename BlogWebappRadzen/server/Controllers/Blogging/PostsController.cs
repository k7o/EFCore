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

  [ODataRoutePrefix("odata/Blogging/Posts")]
  [Route("mvc/odata/Blogging/Posts")]
  public partial class PostsController : ODataController
  {
    private Data.BloggingContext context;

    public PostsController(Data.BloggingContext context)
    {
      this.context = context;
    }
    // GET /odata/Blogging/Posts
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    [HttpGet]
    public IEnumerable<Models.Blogging.Post> GetPosts()
    {
      var items = this.context.Posts.AsQueryable<Models.Blogging.Post>();
      this.OnPostsRead(ref items);

      return items;
    }

    partial void OnPostsRead(ref IQueryable<Models.Blogging.Post> items);

    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    [HttpGet("{PostId}")]
    public SingleResult<Post> GetPost(int key)
    {
        var items = this.context.Posts.Where(i=>i.PostId == key);
        return SingleResult.Create(items);
    }
    partial void OnPostDeleted(Models.Blogging.Post item);
    partial void OnAfterPostDeleted(Models.Blogging.Post item);

    [HttpDelete("{PostId}")]
    public IActionResult DeletePost(int key)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var item = this.context.Posts
                .Where(i => i.PostId == key)
                .FirstOrDefault();

            if (item == null)
            {
                return BadRequest();
            }

            this.OnPostDeleted(item);
            this.context.Posts.Remove(item);
            this.context.SaveChanges();
            this.OnAfterPostDeleted(item);

            return new NoContentResult();
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }

    partial void OnPostUpdated(Models.Blogging.Post item);
    partial void OnAfterPostUpdated(Models.Blogging.Post item);

    [HttpPut("{PostId}")]
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    public IActionResult PutPost(int key, [FromBody]Models.Blogging.Post newItem)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (newItem == null || (newItem.PostId != key))
            {
                return BadRequest();
            }

            this.OnPostUpdated(newItem);
            this.context.Posts.Update(newItem);
            this.context.SaveChanges();

            var itemToReturn = this.context.Posts.Where(i => i.PostId == key);
            Request.QueryString = Request.QueryString.Add("$expand", "Blog");
            this.OnAfterPostUpdated(newItem);
            return new ObjectResult(SingleResult.Create(itemToReturn));
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }

    [HttpPatch("{PostId}")]
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    public IActionResult PatchPost(int key, [FromBody]Delta<Models.Blogging.Post> patch)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = this.context.Posts.Where(i => i.PostId == key).FirstOrDefault();

            if (item == null)
            {
                return BadRequest();
            }

            patch.Patch(item);

            this.OnPostUpdated(item);
            this.context.Posts.Update(item);
            this.context.SaveChanges();

            var itemToReturn = this.context.Posts.Where(i => i.PostId == key);
            Request.QueryString = Request.QueryString.Add("$expand", "Blog");
            return new ObjectResult(SingleResult.Create(itemToReturn));
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }

    partial void OnPostCreated(Models.Blogging.Post item);
    partial void OnAfterPostCreated(Models.Blogging.Post item);

    [HttpPost]
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    public IActionResult Post([FromBody] Models.Blogging.Post item)
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

            this.OnPostCreated(item);
            this.context.Posts.Add(item);
            this.context.SaveChanges();

            var key = item.PostId;

            var itemToReturn = this.context.Posts.Where(i => i.PostId == key);

            Request.QueryString = Request.QueryString.Add("$expand", "Blog");

            this.OnAfterPostCreated(item);

            return new ObjectResult(SingleResult.Create(itemToReturn))
            {
                StatusCode = 201
            };
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }
  }
}
