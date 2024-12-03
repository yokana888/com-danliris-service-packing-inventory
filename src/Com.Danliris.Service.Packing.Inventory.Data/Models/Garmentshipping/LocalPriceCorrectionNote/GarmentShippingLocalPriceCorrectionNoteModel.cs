﻿using Com.Danliris.Service.Packing.Inventory.Data.Models.Garmentshipping.ShippingLocalSalesNote;
using Com.Moonlay.Models;
using System;
using System.Collections.Generic;

namespace Com.Danliris.Service.Packing.Inventory.Data.Models.Garmentshipping.ShippingLocalPriceCorrectionNote
{
    public class GarmentShippingLocalPriceCorrectionNoteModel : StandardEntity
    {
        public string? CorrectionNoteNo { get; private set; }
        public DateTimeOffset CorrectionDate { get; private set; }
        public int SalesNoteId { get; private set; }
        public GarmentShippingLocalSalesNoteModel SalesNote { get; private set; }
        public string? Remark { get; private set; }

        public ICollection<GarmentShippingLocalPriceCorrectionNoteItemModel> Items { get; private set; }

        public GarmentShippingLocalPriceCorrectionNoteModel()
        {
        }

        public GarmentShippingLocalPriceCorrectionNoteModel(string correctionNoteNo, DateTimeOffset correctionDate, int salesNoteId, GarmentShippingLocalSalesNoteModel salesNote, string remark, ICollection<GarmentShippingLocalPriceCorrectionNoteItemModel> items)
        {
            CorrectionNoteNo = correctionNoteNo;
            CorrectionDate = correctionDate;
            SalesNoteId = salesNoteId;
            SalesNote = salesNote;
            Remark = remark;
            Items = items;
        }
    }
}
