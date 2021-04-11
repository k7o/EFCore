using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BlogDL;

namespace BlogWebapp.Pages.BlogPosts
{
    public class CreateModel : PageModel
    {
        private readonly BlogDL.BloggingContext _context;

        public CreateModel(BlogDL.BloggingContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["BlogId"] = new SelectList(_context.Blogs, "BlogId", "Url");
            return Page();
        }

        [BindProperty]
        public Post Post { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Posts.Add(Post);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
