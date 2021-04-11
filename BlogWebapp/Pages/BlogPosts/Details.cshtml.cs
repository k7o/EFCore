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
    public class DetailsModel : PageModel
    {
        private readonly BlogDL.BloggingContext _context;

        public DetailsModel(BlogDL.BloggingContext context)
        {
            _context = context;
        }

        public Post Post { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Post = await _context.Posts
                .Include(p => p.Blog).FirstOrDefaultAsync(m => m.PostId == id);

            if (Post == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
