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
    public partial class EditBlogComponent : ComponentBase
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
        public dynamic BlogId { get; set; }

        BlogWebappRadzen.Models.Blogging.Blog _blog;
        protected BlogWebappRadzen.Models.Blogging.Blog blog
        {
            get
            {
                return _blog;
            }
            set
            {
                if (!object.Equals(_blog, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "blog", NewValue = value, OldValue = _blog };
                    _blog = value;
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
            var bloggingGetBlogByBlogIdResult = await Blogging.GetBlogByBlogId(blogId:BlogId);
            blog = bloggingGetBlogByBlogIdResult;
        }

        protected async System.Threading.Tasks.Task Form0Submit(BlogWebappRadzen.Models.Blogging.Blog args)
        {
            try
            {
                var bloggingUpdateBlogResult = await Blogging.UpdateBlog(blogId:BlogId, blog:blog);
                DialogService.Close(blog);
            }
            catch (System.Exception bloggingUpdateBlogException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to update Blog" });
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
