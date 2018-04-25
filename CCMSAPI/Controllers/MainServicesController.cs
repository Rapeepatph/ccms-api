using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CCMSAPI.DBModels;
using CCMSAPI.Model;
using Newtonsoft.Json;

namespace CCMSAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/MainServices")]
    public class MainServicesController : Controller
    {
        private readonly CCMSAngular5NewContext _context;

        public MainServicesController(CCMSAngular5NewContext context)
        {
            _context = context;
        }

        // GET: api/MainServices
        [HttpGet]
        public IEnumerable<MainServices> GetMainServices()
        {
            return _context.MainServices;
        }

        // GET: api/MainServices/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMainServices([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mainServices = await _context.MainServices.SingleOrDefaultAsync(m => m.Id == id);

            if (mainServices == null)
            {
                return NotFound();
            }

            return Ok(mainServices);
        }

        // GET: api/MainServices/ByBuildingId?buildingId=3
        [HttpGet("ByBuildingId")]
        public IEnumerable<MainServices> ByBuildingId(int buildingId)
        {
            var mainService = _context.MainServices.Where(x => x.BuildingId == buildingId);
            if (mainService == null)
            {
                return null;
            }

            return mainService.ToList();
        }

        // GET: api/MainServices/GetStatus/{serviceId}
        [HttpGet("GetStatus/{serviceId}")]
        public IActionResult GetStatusByServiceId(int serviceId)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var mainService = _context.MainServices.SingleOrDefault(x => x.Id == serviceId);
            if (mainService == null)
            {
                return Ok(null);
            }


            int statusSelectedService = 0;
            int statusStandbyService = 0;
            var service = _context.Services.Where(x => x.MainServiceId == mainService.Id&&x.IsSelected);

            if (service.Count()==0)
            {
                return NotFound();
            }
            else
            {
                statusSelectedService = CheckEachService(service);
            }
            var stdbyService = _context.Services.Where(a => a.MainServiceId == mainService.Id && !a.IsSelected);
            if (stdbyService.Count()==0)
            {
                statusStandbyService = 0;
            }
            else
            {
                statusStandbyService = CheckEachService(stdbyService);
            }
            
            if (statusSelectedService == 1)
            {
                if (statusStandbyService == 2 )
                {
                    return Ok(3);
                }
                else
                {
                    return Ok(1);
                }
            }
            else
            {
                if (statusStandbyService == 1)
                {
                    return Ok(3);
                }
                else
                {
                    return Ok(2);
                }
            }
            
        }

        




        // PUT: api/MainServices/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMainServices([FromRoute] int id, [FromBody] MainServices mainServices)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != mainServices.Id)
            {
                return BadRequest();
            }

            _context.Entry(mainServices).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MainServicesExists(id))
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

        // POST: api/MainServices
        [HttpPost]
        public async Task<IActionResult> PostMainServices([FromBody] MainServices mainServices)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else if (NameServiceExists(mainServices.Name))
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }
            _context.MainServices.Add(mainServices);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMainServices", new { id = mainServices.Id }, mainServices);
        }

        // DELETE: api/MainServices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMainServices([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mainServices = await _context.MainServices.SingleOrDefaultAsync(m => m.Id == id);
            if (mainServices == null)
            {
                return NotFound();
            }

            _context.MainServices.Remove(mainServices);
            await _context.SaveChangesAsync();

            return Ok(mainServices);
        }

        private bool MainServicesExists(int id)
        {
            return _context.MainServices.Any(e => e.Id == id);
        }
        private bool NameServiceExists(string name)
        {
            return _context.MainServices.Any(e => e.Name == name);
        }

        private int CheckEachService(IQueryable<Services> services)
        {

            int DefaultStatus = 0;
            List<int?> sumStatusEachService = new List<int?>();
            if (services != null)
            {
                foreach (var service in services)
                {
                    List<DataEquipment> arrayEquip = JsonConvert.DeserializeObject<List<DataEquipment>>(service.DataEquipment);


                    foreach (var obj in arrayEquip)
                    {
                        var equipment = _context.Equipments.SingleOrDefault(m => m.Name == obj.Name);
                        if (equipment != null)
                        {
                            sumStatusEachService.Add(equipment.Status == null ? 0 : equipment.Status);
                        }
                    }
                }
            }
            else
            {
                sumStatusEachService.Add(1);
            }



            if (sumStatusEachService.Contains(2))
                return 2;
            else if (sumStatusEachService.Contains(3))
                return 3;
            else if (sumStatusEachService.Contains(0))
                return 0;
            //else if (sumStatusEachService.Contains(4))
            //    return Ok(4);
            else if (sumStatusEachService.Contains(1))
                return 1;
            else
                return DefaultStatus;
        }
    }
}