﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Packing.Inventory.Application.CommonViewModelObjectProperties
{
    public class ProductionOrder
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string No { get; set; }
        public string Type { get; set; }
        public double OrderQuantity { get; set; }
        public DateTime CreatedUtc { get; set; }
    }
}
