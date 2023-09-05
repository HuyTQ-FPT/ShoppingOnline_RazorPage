using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebRazor.Hubs;
using WebRazor.Models;

namespace WebRazor.Pages.Admin.Product
{
    public class DeleteModel : PageModel
    {
        private readonly WebRazor.Models.PRN221DBContext _context;
        private readonly IHubContext<HubServer> hubContext;
        public DeleteModel(WebRazor.Models.PRN221DBContext context, IHubContext<HubServer> hubContext)
        {
            _context = context;
            this.hubContext = hubContext;
        }

        [BindProperty]
      public Models.Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FirstOrDefaultAsync(s => s.ProductId == id);
            try
            {
                if (product != null)
                {
                    Product = product;
                    _context.Products.Remove(Product);
                    await _context.SaveChangesAsync();

                }
            }
            catch(Exception ex)
            {
                TempData["key"] = "Product in order can not delete";
                return RedirectToPage("./Index");
            }
           
            //hubContext.Clients.All.SendAsync("ReloadProduct");
            return RedirectToPage("./Index");
        }

    }
}
