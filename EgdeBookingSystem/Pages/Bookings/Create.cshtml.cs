using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using EgdeBookingSystem.Data;
using EgdeBookingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EgdeBookingSystem.Pages.Bookings
{
    public class CreateModel : PageModel
    {
        private readonly EgdeBookingSystem.Data.ApplicationDbContext _context;

        public CreateModel(EgdeBookingSystem.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Equipment Equipment { get; set; }

        public IList<Booking> Bookings { get; set; }
        public IList<DateTime> BookingsDates { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Equipment = await _context.Equipment.AsNoTracking().FirstOrDefaultAsync(m => m.ID == id);

            if (Equipment == null)
            {
                return NotFound();
            }

            Bookings = await _context.Booking.Where(b => b.EquipmentID == id).OrderBy(b => b.StartDate).ToListAsync();

            foreach (Booking b in Bookings)
            {
                for (var dt = b.StartDate; dt <= b.EndDate; dt = dt.AddDays(1))
                {
                    BookingsDates.Add(dt);
                }
            }

            return Page();
        }

        [BindProperty]
        public Booking Booking { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Bookings = await _context.Booking.Where(b => b.EquipmentID == id).OrderBy(b => b.StartDate).ToListAsync();

            if (Bookings != null)
            {
                foreach (Booking b in Bookings)
                {
                    if (b.EquipmentID == Booking.EquipmentID)
                    {
                        if (((Booking.StartDate < b.StartDate) && (Booking.EndDate <= b.StartDate)) || ((Booking.StartDate >= b.EndDate) && (Booking.EndDate > b.EndDate)))
                        {
                            continue;
                        }
                        else
                        {
                            return Page();
                        }
                    }
                }
            }

            _context.Booking.Add(Booking);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
