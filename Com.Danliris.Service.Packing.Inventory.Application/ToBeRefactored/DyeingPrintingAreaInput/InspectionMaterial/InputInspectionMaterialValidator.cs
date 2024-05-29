﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Danliris.Service.Packing.Inventory.Application.ToBeRefactored.DyeingPrintingAreaInput.InspectionMaterial
{
    public class InputInspectionMaterialValidator : AbstractValidator<InputInspectionMaterialViewModel>
    {
        public InputInspectionMaterialValidator()
        {
            RuleFor(data => data.Area).NotNull().WithMessage("Harus Memiliki Area!");
            RuleFor(data => data.Date).Must(s => s != default(DateTimeOffset)).WithMessage("Tanggal Harus Diisi!")
                .Must(s => s >= DateTimeOffset.UtcNow || ((DateTimeOffset.UtcNow - s).TotalDays <= 1 && (DateTimeOffset.UtcNow - s).TotalDays >= 0)).WithMessage("Tanggal Harus Lebih Besar atau Sama Dengan Hari Ini");
            RuleFor(data => data.Shift).NotNull().WithMessage("Shift Harus Diisi!");
            RuleFor(data => data.Group).NotNull().WithMessage("Group Harus Diisi!");
            RuleFor(data => data.InspectionMaterialProductionOrders)
                .Must(s => s.Count > 0).WithMessage("SPP harus Diisi");
            //RuleFor(data => data.InspectionMaterialProductionOrders)
            //    .Must(s => s.GroupBy(d => d.ProductionOrder.Id).All(e => e.Count() == 1))
            //    .WithMessage("SPP harus berbeda setiap detail!")
            //    .When(s => s.InspectionMaterialProductionOrders.All(d => d.ProductionOrder != null));
            RuleForEach(s => s.InspectionMaterialProductionOrders).ChildRules(d =>
            {
                d.RuleFor(data => data.ProductionOrder).NotNull().WithMessage("SPP Harus Diisi!");
                d.When(data => data.ProductionOrder != null, () =>
                {
                    d.RuleFor(s => s.ProductionOrder.Id).GreaterThan(0).WithMessage("SPP Tidak Valid!");
                    d.RuleFor(s => s.ProductionOrder.No).NotNull().WithMessage("SPP Tidak Valid!");
                });
                d.RuleFor(data => data.CartNo).NotNull().WithMessage("No Kereta Harus Diisi!");

            });
        }
    }
}
