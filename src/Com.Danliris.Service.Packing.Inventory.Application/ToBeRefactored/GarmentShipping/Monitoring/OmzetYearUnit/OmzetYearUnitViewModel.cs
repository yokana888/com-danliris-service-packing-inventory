﻿using System;
using System.Collections.Generic;

namespace Com.Danliris.Service.Packing.Inventory.Application.ToBeRefactored.GarmentShipping.Monitoring.OmzetYearUnit
{
    public class OmzetYearUnitViewModel
    {
        public HashSet<string> units { get; internal set; }
        public List<OmzetYearUnitTableViewModel> tables { get; set; }
        public Dictionary<string, decimal> totals { get; set; }
        public Dictionary<string, decimal> averages { get; set; }
    }

    public class OmzetYearUnitTableViewModel
    {
        public string month { get; set; }
        public Dictionary<string, decimal> items { get; set; }
    }

    class JoinedData
    {
        public DateTimeOffset month { get; set; }
        public List<JoinedDataItem> items { get; set; }
    }

    class JoinedDataItem
    {
        public int unit { get; set; }
        public decimal amount { get; set; }
    }

    class SelectedData
    {
        public string month { get; set; }
        public string unit { get; set; }
        public decimal amount { get; set; }
    }
}
