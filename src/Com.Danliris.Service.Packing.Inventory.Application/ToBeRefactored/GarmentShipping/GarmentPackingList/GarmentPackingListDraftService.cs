﻿using Com.Danliris.Service.Packing.Inventory.Data.Models.Garmentshipping.GarmentPackingList;
using System;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Packing.Inventory.Application.ToBeRefactored.GarmentShipping.GarmentPackingList
{
    public class GarmentPackingListDraftService : GarmentPackingListService, IGarmentPackingListDraftService
    {
        public GarmentPackingListDraftService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Task<string> Create(GarmentPackingListViewModel viewModel)
        {
            viewModel.ShippingMarkImagePath = "";
            viewModel.SideMarkImagePath = "";
            viewModel.RemarkImagePath = "";
            viewModel.Status = GarmentPackingListStatusEnum.DRAFT.ToString();
            return base.Create(viewModel);
        }
    }
}
