using System;
using System.Collections.Generic;

namespace CCMSAPI.DBModels
{
    public partial class MainServices
    {
        public MainServices()
        {
            Services = new HashSet<Services>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int BuildingId { get; set; }

        public Buildings Building { get; set; }
        public ICollection<Services> Services { get; set; }
    }
}
