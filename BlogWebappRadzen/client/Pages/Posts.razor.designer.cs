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
    public partial class PostsComponent : ComponentBase
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
        protected RadzenGrid<BlogWebappRadzen.Models.Blogging.Post> grid0;

        string _search;
        protected string search
        {
            get
            {
                return _search;
            }
            set
            {
                if (!object.Equals(_search, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "search", NewValue = value, OldValue = _search };
                    _search = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<BlogWebappRadzen.Models.Blogging.Post> _getPostsResult;
        protected IEnumerable<BlogWebappRadzen.Models.Blogging.Post> getPostsResult
        {
            get
            {
                return _getPostsResult;
            }
            set
            {
                if (!object.Equals(_getPostsResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getPostsResult", NewValue = value, OldValue = _getPostsResult };
                    _getPostsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        int _getPostsCount;
        protected int getPostsCount
        {
            get
            {
                return _getPostsCount;
            }
            set
            {
                if (!object.Equals(_getPostsCount, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getPostsCount", NewValue = value, OldValue = _getPostsCount };
                    _getPostsCount = value;
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
            if (string.IsNullOrEmpty(search)) {
                search = "";
            }
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddPost>("Add Post", null);
            await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Splitbutton0Click(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await Blogging.ExportPostsToCSV(new Query() { Filter = $@"{grid0.Query.Filter}", OrderBy = $"{grid0.Query.OrderBy}", Expand = "Blog", Select = "PostId,Title,Content,Blog.Url" }, $"Posts");

            }

            if (args == null || args.Value == "xlsx")
            {
                await Blogging.ExportPostsToExcel(new Query() { Filter = $@"{grid0.Query.Filter}", OrderBy = $"{grid0.Query.OrderBy}", Expand = "Blog", Select = "PostId,Title,Content,Blog.Url" }, $"Posts");

            }
        }

        protected async System.Threading.Tasks.Task Grid0LoadData(LoadDataArgs args)
        {
            try
            {
                var bloggingGetPostsResult = await Blogging.GetPosts(filter:$@"(contains(Title,""{search}"") or contains(Content,""{search}"")) and {(string.IsNullOrEmpty(args.Filter)? "true" : args.Filter)}", orderby:$"{args.OrderBy}", expand:$"Blog", top:args.Top, skip:args.Skip, count:args.Top != null && args.Skip != null);
                getPostsResult = bloggingGetPostsResult.Value.AsODataEnumerable();

                getPostsCount = bloggingGetPostsResult.Count;
            }
            catch (System.Exception bloggingGetPostsException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to load Posts" });
            }
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(BlogWebappRadzen.Models.Blogging.Post args)
        {
            var dialogResult = await DialogService.OpenAsync<EditPost>("Edit Post", new Dictionary<string, object>() { {"PostId", args.PostId} });
            await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var bloggingDeletePostResult = await Blogging.DeletePost(postId:data.PostId);
                    if (bloggingDeletePostResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (System.Exception bloggingDeletePostException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to delete Post" });
            }
        }
    }
}
