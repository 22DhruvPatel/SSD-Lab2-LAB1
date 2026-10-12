// I, Dhruv Patel, student number 000919050, certify that this material is my
// original work. No other person's work has been used without due
// acknowledgement and I have not made my work available to anyone else.

using LAB1.Data;
using LAB1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace LAB1.Controllers
{
    [Authorize(Roles = "Supervisor,Employee")]
    public class CompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompaniesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // READ - Supervisor + Employee
        public async Task<IActionResult> Index()
        {
            return View(await _context.Companies.ToListAsync());
        }

        // READ - Supervisor + Employee
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .FirstOrDefaultAsync(m => m.Id == id);

            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // CREATE - Supervisor only
        [Authorize(Roles = "Supervisor")]
        public IActionResult Create()
        {
            return View();
        }

        // CREATE - Supervisor only
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> Create(
            [Bind("Id,Name,YearsInBusiness,Website,Province")] Company company)
        {
            if (ModelState.IsValid)
            {
                _context.Add(company);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(company);
        }

        // EDIT - Supervisor only
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies.FindAsync(id);

            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // EDIT - Supervisor only
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Name,YearsInBusiness,Website,Province")] Company company)
        {
            if (id != company.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(company);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(company);
        }

        // DELETE - Supervisor only
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .FirstOrDefaultAsync(m => m.Id == id);

            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // DELETE - Supervisor only
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var company = await _context.Companies.FindAsync(id);

            if (company != null)
            {
                _context.Companies.Remove(company);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.Id == id);
        }
    }
}