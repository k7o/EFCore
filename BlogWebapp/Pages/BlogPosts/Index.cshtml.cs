using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BlogDL;

namespace BlogWebapp.Pages.BlogPosts
{
    public class IndexModel : PageModel
    {
        private readonly BlogDL.BloggingContext _context;

        public IndexModel(BlogDL.BloggingContext context)
        {
            _context = context;
        }

        public IList<Post> Post { get;set; }

        public async Task OnGetAsync()
        {
            Post = await _context.Posts
                .Include(p => p.Blog).ToListAsync();
        }
    }
}
