﻿using System;
using System.Collections.Generic;

namespace CCMSAPI.DBModels
{
    public partial class Buildings
    {
        public Buildings()
        {
            Equipments = new HashSet<Equipments>();
            MainServices = new HashSet<MainServices>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public double? Lng { get; set; }
        public double? Lat { get; set; }

        public ICollection<Equipments> Equipments { get; set; }
        public ICollection<MainServices> MainServices { get; set; }
    }
}
