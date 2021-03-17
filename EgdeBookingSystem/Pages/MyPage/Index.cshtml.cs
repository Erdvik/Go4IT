using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EgdeBookingSystem.Data;
using EgdeBookingSystem.Models;

namespace EgdeBookingSystem.Pages.MyPage
{
    public class IndexModel : PageModel
    {
        private readonly EgdeBookingSystem.Data.ApplicationDbContext _context;

        public IndexModel(EgdeBookingSystem.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Booking> Bookings { get;set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Bookings = await _context.Booking.Where(b => b.UserEmail == id).OrderBy(b => b.StartDate).ToListAsync();

            return Page();
        }
    }
}
