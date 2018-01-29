using System;
using System.Collections.Generic;

namespace CCMSAPI.DBModels
{
    public partial class Services
    {
        public int Id { get; set; }
        public int BuildingId { get; set; }
        public string Name { get; set; }
        public string DataEquipment { get; set; }
    }
}
