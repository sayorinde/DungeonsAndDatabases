﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonsAndDatabases.Models.DND5EAPI
{
    public class AdventureGear : Equipment
    {
        //public string Desc { get; set; }
        public APIReference gear_category { get; set; }
    }
}