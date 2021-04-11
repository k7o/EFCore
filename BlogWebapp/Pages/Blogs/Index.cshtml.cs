using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BlogDL;

namespace BlogWebapp.Pages.Blogs
{
    public class IndexModel : PageModel
    {
        private readonly BlogDL.BloggingContext _context;

        public IndexModel(BlogDL.BloggingContext context)
        {
            _context = context;
        }

        public IList<Blog> Blog { get;set; }

        public async Task OnGetAsync()
        {
            Blog = await _context.Blogs.ToListAsync();
        }
    }
}
