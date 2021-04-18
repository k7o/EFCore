using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using BlogWebappRadzen.Models.Blogging;
using BlogWebappRadzen.Client.Pages;

namespace BlogWebappRadzen.Pages
{
    public partial class EditPostComponent : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager UriHelper { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected BloggingService Blogging { get; set; }

        [Parameter]
        public dynamic PostId { get; set; }

        BlogWebappRadzen.Models.Blogging.Post _post;
        protected BlogWebappRadzen.Models.Blogging.Post post
        {
            get
            {
                return _post;
            }
            set
            {
                if (!object.Equals(_post, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "post", NewValue = value, OldValue = _post };
                    _post = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        BlogWebappRadzen.Models.Blogging.Blog _getByBlogsForBlogIdResult;
        protected BlogWebappRadzen.Models.Blogging.Blog getByBlogsForBlogIdResult
        {
            get
            {
                return _getByBlogsForBlogIdResult;
            }
            set
            {
                if (!object.Equals(_getByBlogsForBlogIdResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getByBlogsForBlogIdResult", NewValue = value, OldValue = _getByBlogsForBlogIdResult };
                    _getByBlogsForBlogIdResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<BlogWebappRadzen.Models.Blogging.Blog> _getBlogsForBlogIdResult;
        protected IEnumerable<BlogWebappRadzen.Models.Blogging.Blog> getBlogsForBlogIdResult
        {
            get
            {
                return _getBlogsForBlogIdResult;
            }
            set
            {
                if (!object.Equals(_getBlogsForBlogIdResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getBlogsForBlogIdResult", NewValue = value, OldValue = _getBlogsForBlogIdResult };
                    _getBlogsForBlogIdResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        int _getBlogsForBlogIdCount;
        protected int getBlogsForBlogIdCount
        {
            get
            {
                return _getBlogsForBlogIdCount;
            }
            set
            {
                if (!object.Equals(_getBlogsForBlogIdCount, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getBlogsForBlogIdCount", NewValue = value, OldValue = _getBlogsForBlogIdCount };
                    _getBlogsForBlogIdCount = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            await Load();
        }
        protected async System.Threading.Tasks.Task Load()
        {
            var bloggingGetPostByPostIdResult = await Blogging.GetPostByPostId(postId:PostId);
            post = bloggingGetPostByPostIdResult;

            if (this.post.BlogId != null)
            {
                var bloggingGetBlogByBlogIdResult = await Blogging.GetBlogByBlogId(blogId:this.post.BlogId);
                getByBlogsForBlogIdResult = bloggingGetBlogByBlogIdResult;
            }
        }

        protected async System.Threading.Tasks.Task Form0Submit(BlogWebappRadzen.Models.Blogging.Post args)
        {
            try
            {
                var bloggingUpdatePostResult = await Blogging.UpdatePost(postId:PostId, post:post);
                DialogService.Close(post);
            }
            catch (System.Exception bloggingUpdatePostException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to update Post" });
            }
        }

        protected async System.Threading.Tasks.Task BlogIdLoadData(LoadDataArgs args)
        {
            var bloggingGetBlogsResult = await Blogging.GetBlogs(filter:$"{args.Filter}", orderby:$"{args.OrderBy}", top:args.Top, skip:args.Skip, count:true);
            getBlogsForBlogIdResult = bloggingGetBlogsResult.Value.AsODataEnumerable();

            getBlogsForBlogIdCount = bloggingGetBlogsResult.Count;
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
