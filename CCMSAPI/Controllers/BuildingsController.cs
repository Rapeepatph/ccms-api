using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CCMSAPI.DBModels;
using Newtonsoft.Json;
using CCMSAPI.Model;

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
        // GET: api/Buildings/GetStatus/{buildingId}
        [HttpGet("GetStatus/{buildingId}")]
        public IActionResult GetStatusByBuildingId(int buildingId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var services = _context.Services.Where(x => x.BuildingId == buildingId);

            if (services == null)
            {
                return NotFound();
            }
            int DefaultStatus = 0;
            List<int> sumStatus = new List<int>();
            foreach (var service in services)
            {
                
                List<DataEquipment> arrayEquip = JsonConvert.DeserializeObject<List<DataEquipment>>(service.DataEquipment);
                
                List<int?> sumStatusEachService = new List<int?>();
                foreach (var obj in arrayEquip)
                {
                    var equipment =  _context.Equipments.SingleOrDefault(m => m.Name == obj.Name);
                    if(equipment != null)
                    {
                        sumStatusEachService.Add(equipment.Status==null?0: equipment.Status);
                    }
                }
                if (sumStatusEachService.Contains(2))
                    sumStatus.Add(2);
                else if (sumStatusEachService.Contains(3))
                    sumStatus.Add(3);
                else if (sumStatusEachService.Contains(0))
                    sumStatus.Add(0);
                //else if (sumStatusEachService.Contains(4))
                //    sumStatus.Add(4);
                else if (sumStatusEachService.Contains(1))
                    sumStatus.Add(1);
                else
                    sumStatus.Add(DefaultStatus);

            }
            if (sumStatus.Contains(2))
                return Ok(2);
            else if (sumStatus.Contains(3))
                return Ok(3);
            else if (sumStatus.Contains(0))
                return Ok(0);
            //else if (sumStatus.Contains(4))
            //    return Ok(4);
            else if (sumStatus.Contains(1))
                return Ok(1);

            return Ok(DefaultStatus);
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