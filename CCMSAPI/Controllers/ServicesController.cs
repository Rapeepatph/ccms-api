using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CCMSAPI.DBModels;
using CCMSAPI.Model;

namespace CCMSAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Services")]
    public class ServicesController : Controller
    {
        private readonly CCMSAngular5TestContext _context;

        public ServicesController(CCMSAngular5TestContext context)
        {
            _context = context;
        }

        // GET: api/Services
        [HttpGet]
        public IEnumerable<Services> GetServices()
        {
            return _context.Services;
        }

        // GET: api/Services/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetServices([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var services = await _context.Services.SingleOrDefaultAsync(m => m.Id == id);

            if (services == null)
            {
                return NotFound();
            }

            return Ok(services);
        }
        [HttpGet("ByBuildingId")]
        public IEnumerable<Services> ByBuildingId(int buildingId)
        {
            var res = _context.Services.Where(x => x.BuildingId == buildingId);
            return res.ToList();
        }
        // PUT: api/Services/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServices([FromRoute] int id, [FromBody] Services services)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != services.Id)
            {
                return BadRequest();
            }

            _context.Entry(services).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServicesExists(id))
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

        // POST: api/Services
        [HttpPost]
        public async Task<IActionResult> PostServices([FromBody] Services services)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Services.Add(services);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                if (ServicesExists(services.Id))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetServices", new { id = services.Id }, services);
        }

        // DELETE: api/Services/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServices([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var services = await _context.Services.SingleOrDefaultAsync(m => m.Id == id);
            if (services == null)
            {
                return NotFound();
            }

            _context.Services.Remove(services);
            await _context.SaveChangesAsync();

            return Ok(services);
        }

        private bool ServicesExists(int id)
        {
            return _context.Services.Any(e => e.Id == id);
        }
    }
}