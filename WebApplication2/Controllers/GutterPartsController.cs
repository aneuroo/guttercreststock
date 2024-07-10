using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class GutterPartsController : Controller
    {
        private readonly WebApplication2Context _context;

        public GutterPartsController(WebApplication2Context context)
        {
            _context = context;
        }

        // GET: GutterParts
        public async Task<IActionResult> Index()
        {
            return View(await _context.GutterPart.ToListAsync());
        }

        // GET: GutterParts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gutterPart = await _context.GutterPart
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gutterPart == null)
            {
                return NotFound();
            }

            return View(gutterPart);
        }

        // GET: GutterParts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GutterParts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,quantity,Location,Sku")] GutterPart gutterPart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gutterPart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gutterPart);
        }

        // GET: GutterParts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gutterPart = await _context.GutterPart.FindAsync(id);
            if (gutterPart == null)
            {
                return NotFound();
            }
            return View(gutterPart);
        }

        // POST: GutterParts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,quantity,Location,Sku")] GutterPart gutterPart)
        {
            if (id != gutterPart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gutterPart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GutterPartExists(gutterPart.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(gutterPart);
        }

        // GET: GutterParts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gutterPart = await _context.GutterPart
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gutterPart == null)
            {
                return NotFound();
            }

            return View(gutterPart);
        }

        // POST: GutterParts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gutterPart = await _context.GutterPart.FindAsync(id);
            if (gutterPart != null)
            {
                _context.GutterPart.Remove(gutterPart);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GutterPartExists(int id)
        {
            return _context.GutterPart.Any(e => e.Id == id);
        }
    }
}
