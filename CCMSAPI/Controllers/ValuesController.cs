using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCMSAPI.DBModels;
using CCMSAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace CCMSAPI.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<MarkerJSON> Get()
        {
            var res = new List<MarkerJSON>();
            var newMarker = new MarkerJSON { ID = 1, Name = "Tower", Lng = 102.777419, Lat = 17.386123 };
            var newMarker2 = new MarkerJSON { ID = 2, Name = "SSR", Lng = 102.769973, Lat = 17.387600 };
            res.Add(newMarker);
            res.Add(newMarker2);
            return res;


        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
