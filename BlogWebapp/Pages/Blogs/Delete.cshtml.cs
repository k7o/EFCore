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
    public class DeleteModel : PageModel
    {
        private readonly BlogDL.BloggingContext _context;

        public DeleteModel(BlogDL.BloggingContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Blog Blog { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Blog = await _context.Blogs.FirstOrDefaultAsync(m => m.BlogId == id);

            if (Blog == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Blog = await _context.Blogs.FindAsync(id);

            if (Blog != null)
            {
                _context.Blogs.Remove(Blog);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
