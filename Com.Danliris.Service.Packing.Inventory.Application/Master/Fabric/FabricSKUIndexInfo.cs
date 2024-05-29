﻿using System;

namespace Com.Danliris.Service.Packing.Inventory.Application.Master.Fabric
{
    public class FabricSKUIndexInfo
    {
        public FabricSKUIndexInfo()
        {
        }

        public int Id { get; set; }
        public DateTime LastModifiedUtc { get; set; }
        public string WovenType { get; set; }
        public string Construction { get; set; }
        public string Width { get; set; }
        public string Warp { get; set; }
        public string Weft { get; set; }
        public string ProcessType { get; set; }
        public string YarnType { get; set; }
        public string Grade { get; set; }
        public string UOM { get; set; }
        public string Code { get; set; }
    }
}