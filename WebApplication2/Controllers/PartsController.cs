using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class PartsController : Controller
    {
        private readonly WebApplication2Context _context;


        public PartsController(WebApplication2Context context)
        {
            _context = context;
        }

        // GET: Parts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Parts.ToListAsync());
        }
        public async Task<IActionResult> Editlist(string? sku)
        {
            var Parts = from m in _context.Parts
                        select m;

            if (!String.IsNullOrEmpty(sku))
            {
                Parts = Parts.Where(s => s.Sku!.Equals(sku));
            }
            var orderID = Request.Query["orderid"];
            return View(await Parts.ToListAsync());

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editlist(int id, [Bind("Id,quantity,Location,Sku")] Parts parts)
        {
            if (id != parts.Id)
            {
                return NotFound();
            }
            var orderID = Request.Form["orderid"].ToString(); 
            var strRequired = Request.Form["strRequired"].ToString();
            var oldCount = Request.Form["oldCount"].ToString();
            var iRequired = Int32.Parse(strRequired);

            var Orders = from p in _context.Orders
                         select p;

            if (!String.IsNullOrEmpty(orderID))
            {
                Orders = Orders.Where(s => s.OrderId!.Equals(orderID));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var PartsCount = from m in _context.Parts
                                     select m;
                    var iCountOld = parts.quantity;
                    if (!String.IsNullOrEmpty(parts.Sku))
                    {
                        PartsCount = PartsCount.Where(s => s.Sku!.Contains(parts.Sku));
             
                    }
                    //get diff


                    //iCount = 0;
                    _context.Update(parts);

                    var iCount = Int32.Parse(oldCount) - parts.quantity; //count to remove
                    iRequired = (Int32.Parse(strRequired) - iCount);
                    var result = _context.Orders.SingleOrDefault(o => o.OrderId == orderID);
                    if (result != null)
                    {
                        int index = result.Parts.FindIndex(a => a == parts.Sku);

                        result.Required[index] = result.Required[index] - iCount;
                        //result.Required = [1,1];

                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartsExists(parts.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction(nameof(Index));
                var Parts = from m in _context.Parts
                            select m;

                if (!String.IsNullOrEmpty(parts.Sku))
                {
                    Parts = Parts.Where(s => s.Sku!.Contains(parts.Sku));
                }


                return View(await Parts.ToListAsync());

            }


            return View(parts);



        }
        // GET: Parts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parts = await _context.Parts.FindAsync(id);
            if (parts == null)
            {
                return NotFound();
            }
            return View(parts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,quantity,Location,Sku")] Parts parts)
        {
            if (id != parts.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartsExists(parts.Id))
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
            return View(parts);
        }

        // GET: Parts/Delete/5


        // GET: Parts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parts = await _context.Parts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parts == null)
            {
                return NotFound();
            }

            return View(parts);
        }

        // GET: Parts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Parts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,quantity,Location,Sku")] Parts parts)
        {
            if (ModelState.IsValid)
            {
                _context.Add(parts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(parts);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parts = await _context.Parts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parts == null)
            {
                return NotFound();
            }

            return View(parts);
        }

        // POST: Parts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parts = await _context.Parts.FindAsync(id);
            if (parts != null)
            {
                _context.Parts.Remove(parts);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PartsExists(int id)
        {
            return _context.Parts.Any(e => e.Id == id);
        }
    }
}
