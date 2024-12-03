﻿using Com.Moonlay.Models;
using System.ComponentModel.DataAnnotations;

namespace Com.Danliris.Service.Packing.Inventory.Data.Models.ProductByDivisionOrCategory
{
    public class FabricProductPackingModel : StandardEntity
    {
        public FabricProductPackingModel()
        {

        }

        public FabricProductPackingModel(
            string code,
            int fabricProductSKUId,
            int productSKUId,
            int productPackingId,
            int uomId,
            double packingSize
            )
        {
            Code = code;
            FabricProductSKUId = fabricProductSKUId;
            ProductSKUId = productSKUId;
            ProductPackingId = productPackingId;
            UOMId = uomId;
            PackingSize = packingSize;
        }

        public FabricProductPackingModel(
            string code,
            int fabricProductSKUId,
            int productSKUId,
            int productPackingId,
            int uomId,
            double packingSize,
            string packingType,
            bool afterStockOpname,
            string description
            )
        {
            Code = code;
            FabricProductSKUId = fabricProductSKUId;
            ProductSKUId = productSKUId;
            ProductPackingId = productPackingId;
            UOMId = uomId;
            PackingSize = packingSize;
            PackingType = packingType;
            AfterStockOpname = afterStockOpname;
            Description = description;
        }

        [MaxLength(64)]
        public string? Code { get; private set; }
        public int FabricProductSKUId { get; private set; }
        public int ProductSKUId { get; private set; }
        public int ProductPackingId { get; private set; }
        public int UOMId { get; private set; }
        public double PackingSize { get; private set; }
        [MaxLength(64)]
        public string? PackingType { get; private set; }
        public bool AfterStockOpname { get; set; }
        public string? Description { get; set; }
    }
}
