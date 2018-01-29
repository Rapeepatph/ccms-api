using System;
using System.Collections.Generic;

namespace CCMSAPI.DBModels
{
    public partial class Equipments
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BuildingId { get; set; }

        public Buildings Building { get; set; }
    }
}
