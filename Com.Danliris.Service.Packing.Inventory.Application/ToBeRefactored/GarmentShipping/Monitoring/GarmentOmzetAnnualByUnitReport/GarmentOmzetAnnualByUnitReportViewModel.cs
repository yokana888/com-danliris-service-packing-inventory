﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Packing.Inventory.Application.ToBeRefactored.GarmentShipping.Monitoring.GarmentOmzetAnnualByUnitReport
{
    public class GarmentDetailOmzetByUnitReportViewModel
    {
        public string InvoiceNo { get; set; }
        public string UnitCode { get; set; }
        public string UnitName { get; set; }
        public string RONumber { get; set; }
        public DateTimeOffset PEBDate { get; set; }
        public DateTimeOffset TruckingDate { get; set; }
        public string Month { get; set; }
        public string MonthName { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Rate { get; set; }
        public decimal AmountIDR { get; set; }
    }

    class CurrencyFilter
    {
        public DateTime date { get; set; }
        public string code { get; set; }
    }

    public class AnnualOmzetByUnitViewModel
    {
        public string Month { get; set; }
        public string MonthName { get; set; }
        public decimal Amount1 { get; set; }
        public decimal Amount1IDR { get; set; }
        public decimal Amount2 { get; set; }
        public decimal Amount2IDR { get; set; }
        public decimal Amount3 { get; set; }
        public decimal Amount3IDR { get; set; }
        public decimal Amount4 { get; set; }
        public decimal Amount4IDR { get; set; }
        public decimal Amount5 { get; set; }
        public decimal Amount5IDR { get; set; }
    }
}
