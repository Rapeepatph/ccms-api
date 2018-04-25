using System;
using System.Collections.Generic;

namespace CCMSAPI.DBModels
{
    public partial class Services
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MainServiceId { get; set; }
        public string DataEquipment { get; set; }
        public bool IsSelected { get; set; }

        public MainServices MainService { get; set; }
    }
}
