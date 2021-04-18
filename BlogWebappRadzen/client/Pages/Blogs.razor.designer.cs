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
    public partial class BlogsComponent : ComponentBase
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
        protected RadzenGrid<BlogWebappRadzen.Models.Blogging.Blog> grid0;

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

        IEnumerable<BlogWebappRadzen.Models.Blogging.Blog> _getBlogsResult;
        protected IEnumerable<BlogWebappRadzen.Models.Blogging.Blog> getBlogsResult
        {
            get
            {
                return _getBlogsResult;
            }
            set
            {
                if (!object.Equals(_getBlogsResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getBlogsResult", NewValue = value, OldValue = _getBlogsResult };
                    _getBlogsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        int _getBlogsCount;
        protected int getBlogsCount
        {
            get
            {
                return _getBlogsCount;
            }
            set
            {
                if (!object.Equals(_getBlogsCount, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getBlogsCount", NewValue = value, OldValue = _getBlogsCount };
                    _getBlogsCount = value;
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
            var dialogResult = await DialogService.OpenAsync<AddBlog>("Add Blog", null);
            await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Splitbutton0Click(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await Blogging.ExportBlogsToCSV(new Query() { Filter = $@"{grid0.Query.Filter}", OrderBy = $"{grid0.Query.OrderBy}", Expand = "", Select = "BlogId,Url" }, $"Blogs");

            }

            if (args == null || args.Value == "xlsx")
            {
                await Blogging.ExportBlogsToExcel(new Query() { Filter = $@"{grid0.Query.Filter}", OrderBy = $"{grid0.Query.OrderBy}", Expand = "", Select = "BlogId,Url" }, $"Blogs");

            }
        }

        protected async System.Threading.Tasks.Task Grid0LoadData(LoadDataArgs args)
        {
            try
            {
                var bloggingGetBlogsResult = await Blogging.GetBlogs(filter:$@"(contains(Url,""{search}"")) and {(string.IsNullOrEmpty(args.Filter)? "true" : args.Filter)}", orderby:$"{args.OrderBy}", top:args.Top, skip:args.Skip, count:args.Top != null && args.Skip != null);
                getBlogsResult = bloggingGetBlogsResult.Value.AsODataEnumerable();

                getBlogsCount = bloggingGetBlogsResult.Count;
            }
            catch (System.Exception bloggingGetBlogsException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to load Blogs" });
            }
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(BlogWebappRadzen.Models.Blogging.Blog args)
        {
            var dialogResult = await DialogService.OpenAsync<EditBlog>("Edit Blog", new Dictionary<string, object>() { {"BlogId", args.BlogId} });
            await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var bloggingDeleteBlogResult = await Blogging.DeleteBlog(blogId:data.BlogId);
                    if (bloggingDeleteBlogResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (System.Exception bloggingDeleteBlogException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to delete Blog" });
            }
        }
    }
}
