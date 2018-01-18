using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CCMSAPI.DBModels;

namespace CCMSAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Buildings")]
    public class BuildingsController : Controller
    {
        private readonly CCMSAngular5TestContext _context;

        public BuildingsController(CCMSAngular5TestContext context)
        {
            _context = context;
        }

        // GET: api/Buildings
        [HttpGet]
        public IEnumerable<Buildings> GetBuildings()
        {
            return _context.Buildings;
        }

        // GET: api/Buildings/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBuildings([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var buildings = await _context.Buildings.SingleOrDefaultAsync(m => m.Id == id);

            if (buildings == null)
            {
                return NotFound();
            }

            return Ok(buildings);
        }

        // PUT: api/Buildings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBuildings([FromRoute] int id, [FromBody] Buildings buildings)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != buildings.Id)
            {
                return BadRequest();
            }

            _context.Entry(buildings).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BuildingsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Buildings
        [HttpPost]
        public async Task<IActionResult> PostBuildings([FromBody] Buildings buildings)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Buildings.Add(buildings);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBuildings", new { id = buildings.Id }, buildings);
        }

        // DELETE: api/Buildings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBuildings([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var buildings = await _context.Buildings.SingleOrDefaultAsync(m => m.Id == id);
            if (buildings == null)
            {
                return NotFound();
            }

            _context.Buildings.Remove(buildings);
            await _context.SaveChangesAsync();

            return Ok(buildings);
        }

        private bool BuildingsExists(int id)
        {
            return _context.Buildings.Any(e => e.Id == id);
        }
    }
}