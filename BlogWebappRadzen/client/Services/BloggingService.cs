
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Web;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Components;
using Radzen;
using BlogWebappRadzen.Models.Blogging;

namespace BlogWebappRadzen
{
    public partial class BloggingService
    {
        private readonly HttpClient httpClient;
        private readonly Uri baseUri;
        private readonly NavigationManager navigationManager;
        public BloggingService(NavigationManager navigationManager, HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;

            this.navigationManager = navigationManager;
            this.baseUri = new Uri($"{navigationManager.BaseUri}odata/Blogging/");
        }

        public async System.Threading.Tasks.Task ExportBlogsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/blogging/blogs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/blogging/blogs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportBlogsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/blogging/blogs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/blogging/blogs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }
        partial void OnGetBlogs(HttpRequestMessage requestMessage);


        public async System.Threading.Tasks.Task<ODataServiceResult<Blog>> GetBlogs(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string))
        {
            var uri = new Uri(baseUri, $"Blogs");
            uri = uri.GetODataUri(filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:null, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetBlogs(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await response.ReadAsync<ODataServiceResult<Blog>>();
        }
        partial void OnCreateBlog(HttpRequestMessage requestMessage);


        public async System.Threading.Tasks.Task<Blog> CreateBlog(Blog blog = default(Blog))
        {
            var uri = new Uri(baseUri, $"Blogs");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);


            httpRequestMessage.Content = new StringContent(ODataJsonSerializer.Serialize(blog), Encoding.UTF8, "application/json");

            OnCreateBlog(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await response.ReadAsync<Blog>();
        }

        public async System.Threading.Tasks.Task ExportPostsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/blogging/posts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/blogging/posts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportPostsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/blogging/posts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/blogging/posts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }
        partial void OnGetPosts(HttpRequestMessage requestMessage);


        public async System.Threading.Tasks.Task<ODataServiceResult<Post>> GetPosts(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string))
        {
            var uri = new Uri(baseUri, $"Posts");
            uri = uri.GetODataUri(filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:null, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetPosts(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await response.ReadAsync<ODataServiceResult<Post>>();
        }
        partial void OnCreatePost(HttpRequestMessage requestMessage);


        public async System.Threading.Tasks.Task<Post> CreatePost(Post post = default(Post))
        {
            var uri = new Uri(baseUri, $"Posts");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);


            httpRequestMessage.Content = new StringContent(ODataJsonSerializer.Serialize(post), Encoding.UTF8, "application/json");

            OnCreatePost(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await response.ReadAsync<Post>();
        }
        partial void OnDeleteBlog(HttpRequestMessage requestMessage);


        public async System.Threading.Tasks.Task<HttpResponseMessage> DeleteBlog(int? blogId = default(int?))
        {
            var uri = new Uri(baseUri, $"Blogs({blogId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteBlog(httpRequestMessage);
            return await httpClient.SendAsync(httpRequestMessage);
        }
        partial void OnGetBlogByBlogId(HttpRequestMessage requestMessage);


        public async System.Threading.Tasks.Task<Blog> GetBlogByBlogId(int? blogId = default(int?))
        {
            var uri = new Uri(baseUri, $"Blogs({blogId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetBlogByBlogId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await response.ReadAsync<Blog>();
        }
        partial void OnUpdateBlog(HttpRequestMessage requestMessage);


        public async System.Threading.Tasks.Task<HttpResponseMessage> UpdateBlog(int? blogId = default(int?), Blog blog = default(Blog))
        {
            var uri = new Uri(baseUri, $"Blogs({blogId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(ODataJsonSerializer.Serialize(blog), Encoding.UTF8, "application/json");

            OnUpdateBlog(httpRequestMessage);
            return await httpClient.SendAsync(httpRequestMessage);
        }
        partial void OnDeletePost(HttpRequestMessage requestMessage);


        public async System.Threading.Tasks.Task<HttpResponseMessage> DeletePost(int? postId = default(int?))
        {
            var uri = new Uri(baseUri, $"Posts({postId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeletePost(httpRequestMessage);
            return await httpClient.SendAsync(httpRequestMessage);
        }
        partial void OnGetPostByPostId(HttpRequestMessage requestMessage);


        public async System.Threading.Tasks.Task<Post> GetPostByPostId(int? postId = default(int?))
        {
            var uri = new Uri(baseUri, $"Posts({postId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetPostByPostId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await response.ReadAsync<Post>();
        }
        partial void OnUpdatePost(HttpRequestMessage requestMessage);


        public async System.Threading.Tasks.Task<HttpResponseMessage> UpdatePost(int? postId = default(int?), Post post = default(Post))
        {
            var uri = new Uri(baseUri, $"Posts({postId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(ODataJsonSerializer.Serialize(post), Encoding.UTF8, "application/json");

            OnUpdatePost(httpRequestMessage);
            return await httpClient.SendAsync(httpRequestMessage);
        }
    }
}
