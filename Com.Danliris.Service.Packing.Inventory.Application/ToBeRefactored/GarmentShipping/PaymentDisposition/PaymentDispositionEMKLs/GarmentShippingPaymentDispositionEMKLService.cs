﻿
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Com.Danliris.Service.Packing.Inventory.Application.Utilities;
using Com.Danliris.Service.Packing.Inventory.Infrastructure.Repositories.GarmentShipping.PaymentDisposition;
using Com.Danliris.Service.Packing.Inventory.Data.Models.Garmentshipping.PaymentDisposition;
using System.Linq;
using System.Collections.Generic;
using Com.Danliris.Service.Packing.Inventory.Application.ToBeRefactored.CommonViewModelObjectProperties;
using Com.Danliris.Service.Packing.Inventory.Application.CommonViewModelObjectProperties;
using Newtonsoft.Json;
using Com.Danliris.Service.Packing.Inventory.Infrastructure.Utilities;

namespace Com.Danliris.Service.Packing.Inventory.Application.ToBeRefactored.GarmentShipping.PaymentDisposition.PaymentDispositionEMKLs
{
    public class GarmentShippingPaymentDispositionEMKLService : IGarmentShippingPaymentEMKLDispositionService
    {
        private readonly IGarmentShippingPaymentDispositionRepository _repository;
        private readonly IServiceProvider serviceProvider;

        public GarmentShippingPaymentDispositionEMKLService(IServiceProvider serviceProvider)
        {
            _repository = serviceProvider.GetService<IGarmentShippingPaymentDispositionRepository>();

            this.serviceProvider = serviceProvider;
        }

        public GarmentShippingPaymentDispositionEMKLViewModel MapToViewModel(GarmentShippingPaymentDispositionModel model)
        {
            GarmentShippingPaymentDispositionEMKLViewModel viewModel = new GarmentShippingPaymentDispositionEMKLViewModel
            {
                Active = model.Active,
                Id = model.Id,
                CreatedAgent = model.CreatedAgent,
                CreatedBy = model.CreatedBy,
                CreatedUtc = model.CreatedUtc,
                DeletedAgent = model.DeletedAgent,
                DeletedBy = model.DeletedBy,
                DeletedUtc = model.DeletedUtc,
                IsDeleted = model.IsDeleted,
                LastModifiedAgent = model.LastModifiedAgent,
                LastModifiedBy = model.LastModifiedBy,
                LastModifiedUtc = model.LastModifiedUtc,

                dispositionNo = model.DispositionNo,
                accNo = model.AccNo,
                address = model.Address,
                paidAt = model.PaidAt,
                buyerAgent = new BuyerAgent
                {
                    Name = "",
                    Id = 0,
                    Code = ""
                },
                bank = model.Bank,
                billValue = model.BillValue,
                courier = new Courier
                {
                    Name = model.CourierName,
                    Code = model.CourierCode,
                    Id = model.CourierId
                },
                emkl = new EMKL
                {
                    Name = model.EMKLName,
                    Code = model.EMKLCode,
                    Id = model.EMKLId
                },
                forwarder = new Forwarder
                {
                    name = model.ForwarderName,
                    code = model.ForwarderCode,
                    id = model.ForwarderId
                },
                warehouse = new WareHouse
                {
                    Name = model.WareHouseName,
                    Code = model.WareHouseCode,
                    Id = model.WareHouseId
                },
                freightBy = model.FreightBy,
                freightDate = model.FreightDate,
                freightNo = model.FreightNo,
                incomeTax = new IncomeTax
                {
                    id = model.IncomeTaxId,
                    name = model.IncomeTaxName,
                    rate = (double)model.IncomeTaxRate
                },
                IncomeTaxValue = model.IncomeTaxValue,
                invoiceDate = model.InvoiceDate,
                invoiceNumber = model.InvoiceNumber,
                invoiceTaxNumber = model.InvoiceTaxNumber,
                isFreightCharged = model.IsFreightCharged,
                npwp = model.NPWP,
                paymentDate = model.PaymentDate,
                paymentMethod = model.PaymentMethod,
                paymentTerm = model.PaymentTerm,
                paymentType = model.PaymentType,
                remark = model.Remark,
                sendBy = model.SendBy,
                totalBill = model.TotalBill,
                vatValue = model.VatValue,
                flightVessel = model.FlightVessel,
                destination=model.Destination,
                unitCharges = (model.UnitCharges ?? new List<GarmentShippingPaymentDispositionUnitChargeModel>()).Select(i => new GarmentShippingPaymentDispositionUnitChargeViewModel
                {
                    Active = i.Active,
                    Id = i.Id,
                    CreatedAgent = i.CreatedAgent,
                    CreatedBy = i.CreatedBy,
                    CreatedUtc = i.CreatedUtc,
                    DeletedAgent = i.DeletedAgent,
                    DeletedBy = i.DeletedBy,
                    DeletedUtc = i.DeletedUtc,
                    IsDeleted = i.IsDeleted,
                    LastModifiedAgent = i.LastModifiedAgent,
                    LastModifiedBy = i.LastModifiedBy,
                    LastModifiedUtc = i.LastModifiedUtc,

                    amountPercentage = i.AmountPercentage,
                    billAmount = i.BillAmount,
                    paymentDispositionId = i.PaymentDispositionId,
                    unit = new Unit
                    {
                        Id = i.UnitId,
                        Code = i.UnitCode
                    }
                }).ToList(),
                invoiceDetails = (model.InvoiceDetails ?? new List<GarmentShippingPaymentDispositionInvoiceDetailModel>()).Select(i => new GarmentShippingPaymentDispositionEMKLInvoiceDetailViewModel
                {
                    Active = i.Active,
                    Id = i.Id,
                    CreatedAgent = i.CreatedAgent,
                    CreatedBy = i.CreatedBy,
                    CreatedUtc = i.CreatedUtc,
                    DeletedAgent = i.DeletedAgent,
                    DeletedBy = i.DeletedBy,
                    DeletedUtc = i.DeletedUtc,
                    IsDeleted = i.IsDeleted,
                    LastModifiedAgent = i.LastModifiedAgent,
                    LastModifiedBy = i.LastModifiedBy,
                    LastModifiedUtc = i.LastModifiedUtc,

                    amount = i.Amount,
                    chargeableWeight = i.ChargeableWeight,
                    grossWeight = i.GrossWeight,
                    invoiceId = i.InvoiceId,
                    invoiceNo = i.InvoiceNo,
                    paymentDispositionId = i.PaymentDispositionId,
                    quantity = i.Quantity,
                    totalCarton = i.TotalCarton,
                    volume = i.Volume,

                    buyerAgent = new BuyerAgent
                    {
                        Name = i.BuyerAgentName,
                        Id = i.BuyerAgentId.Value,
                        Code = i.BuyerAgentCode
                    },

                }).ToList(),
                billDetails = (model.BillDetails ?? new List<GarmentShippingPaymentDispositionBillDetailModel>()).Select(i => new GarmentShippingPaymentDispositionBillDetailViewModel
                {
                    Active = i.Active,
                    Id = i.Id,
                    CreatedAgent = i.CreatedAgent,
                    CreatedBy = i.CreatedBy,
                    CreatedUtc = i.CreatedUtc,
                    DeletedAgent = i.DeletedAgent,
                    DeletedBy = i.DeletedBy,
                    DeletedUtc = i.DeletedUtc,
                    IsDeleted = i.IsDeleted,
                    LastModifiedAgent = i.LastModifiedAgent,
                    LastModifiedBy = i.LastModifiedBy,
                    LastModifiedUtc = i.LastModifiedUtc,                    
                    amount = i.Amount,
                    billDescription = i.BillDescription

                }).ToList(),
                paymentDetails = (model.PaymentDetails ?? new List<GarmentShippingPaymentDispositionPaymentDetailModel>()).Select(i => new GarmentShippingPaymentDispositionPaymentDetailViewModel
                {
                    Active = i.Active,
                    Id = i.Id,
                    CreatedAgent = i.CreatedAgent,
                    CreatedBy = i.CreatedBy,
                    CreatedUtc = i.CreatedUtc,
                    DeletedAgent = i.DeletedAgent,
                    DeletedBy = i.DeletedBy,
                    DeletedUtc = i.DeletedUtc,
                    IsDeleted = i.IsDeleted,
                    LastModifiedAgent = i.LastModifiedAgent,
                    LastModifiedBy = i.LastModifiedBy,
                    LastModifiedUtc = i.LastModifiedUtc,

                    paymentDate = i.PaymentDate,
                    amount = i.Amount,
                    paymentDescription = i.PaymentDescription

                }).ToList()
            };

            return viewModel;
        }

        public GarmentShippingPaymentDispositionModel MapToModel(GarmentShippingPaymentDispositionEMKLViewModel vm)
        {
            var bills = (vm.billDetails ?? new List<GarmentShippingPaymentDispositionBillDetailViewModel>()).Select(i =>
            {
                return new GarmentShippingPaymentDispositionBillDetailModel(i.billDescription,i.amount) { Id = i.Id };
            }).ToList();

            var units= (vm.unitCharges ?? new List<GarmentShippingPaymentDispositionUnitChargeViewModel>()).Select(m => {

                m.unit = m.unit ?? new Unit();
                return new GarmentShippingPaymentDispositionUnitChargeModel(m.unit.Id, m.unit.Code, m.amountPercentage, m.billAmount)
                {
                    Id = m.Id
                };
            }).ToList();

            var invoices = (vm.invoiceDetails ?? new List<GarmentShippingPaymentDispositionEMKLInvoiceDetailViewModel>()).Select(i =>
            {
                i.buyerAgent = i.buyerAgent ?? new BuyerAgent();
                return new GarmentShippingPaymentDispositionInvoiceDetailModel(i.invoiceNo,i.invoiceId,i.quantity, i.amount,i.volume,i.grossWeight,i.chargeableWeight,i.totalCarton,i.buyerAgent.Id, i.buyerAgent.Name, i.buyerAgent.Code) { Id = i.Id };
            }).ToList();

            var payments = (vm.paymentDetails ?? new List<GarmentShippingPaymentDispositionPaymentDetailViewModel>()).Select(i =>
            {
                return new GarmentShippingPaymentDispositionPaymentDetailModel(i.paymentDate, i.paymentDescription, i.amount) { Id = i.Id };
            }).ToList();

            vm.forwarder = vm.forwarder ?? new Forwarder();
            //vm.buyerAgent = vm.buyerAgent ?? new BuyerAgent();
            vm.emkl = vm.emkl ?? new EMKL();
            vm.courier = vm.courier ?? new Courier();
            vm.warehouse = vm.warehouse ?? new WareHouse();
            vm.incomeTax = vm.incomeTax ?? new IncomeTax();
            return new GarmentShippingPaymentDispositionModel(GenerateNo(vm), vm.paymentType, vm.paymentMethod, vm.paidAt, vm.sendBy,
                0,"","", vm.paymentTerm, vm.forwarder.id, vm.forwarder.code, vm.forwarder.name,
                vm.courier.Id, vm.courier.Code, vm.courier.Name, vm.emkl.Id, vm.emkl.Code, vm.emkl.Name, vm.warehouse.Id, vm.warehouse.Code, vm.warehouse.Name, vm.address, vm.npwp, vm.invoiceNumber, vm.invoiceDate,
                vm.invoiceTaxNumber, vm.billValue, vm.vatValue, vm.incomeTax.id, vm.incomeTax.name, (decimal)vm.incomeTax.rate, vm.IncomeTaxValue,
                vm.totalBill, vm.paymentDate, vm.bank, vm.accNo, vm.isFreightCharged, vm.freightBy, vm.freightNo, vm.freightDate.GetValueOrDefault(), vm.flightVessel, vm.destination, vm.remark, invoices, bills, units, payments)
            { Id = vm.Id };
        }

        private string GenerateNo(GarmentShippingPaymentDispositionEMKLViewModel vm)
        {
            var year = DateTime.Now.ToString("yy");

            var prefix = $"DL/EMKL/{year}/"; ;
          
            var lastInvoiceNo = _repository.ReadAll().Where(w => w.DispositionNo.StartsWith(prefix))
                .OrderByDescending(o => o.DispositionNo)
                .Select(s => int.Parse(s.DispositionNo.Replace(prefix, "")))
                .FirstOrDefault();
            var invoiceNo = $"{prefix}{(lastInvoiceNo + 1).ToString("D4")}";

            return invoiceNo;
        }

        public async Task<int> Create(GarmentShippingPaymentDispositionEMKLViewModel viewModel)
        {
            var model = MapToModel(viewModel);

            int Created = await _repository.InsertAsync(model);

            return Created;
        }

        public async Task<int> Delete(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public ListResult<GarmentShippingPaymentDispositionEMKLViewModel> Read(int page, int size, string filter, string order, string keyword)
        {
            var query = _repository.ReadAll();
            List<string> SearchAttributes = new List<string>()
            {
                "DispositionNo", "BuyerAgentCode", "BuyerAgentName", "PaymentType","ForwarderName","ForwarderCode","EMKLName",
                "EMKLCode","CourierName","CourierCode","WareHouseName","WareHouseCode"
            };
            query = QueryHelper<GarmentShippingPaymentDispositionModel>.Search(query, SearchAttributes, keyword);

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            query = QueryHelper<GarmentShippingPaymentDispositionModel>.Filter(query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<GarmentShippingPaymentDispositionModel>.Order(query, OrderDictionary);

            var data = query
                .Skip((page - 1) * size)
                .Take(size)
                .Select(model => MapToViewModel(model))
                .ToList();

            return new ListResult<GarmentShippingPaymentDispositionEMKLViewModel>(data, page, size, query.Count());
        }

        public async Task<GarmentShippingPaymentDispositionEMKLViewModel> ReadById(int id)
        {
            var data = await _repository.ReadByIdAsync(id);

            var viewModel = MapToViewModel(data);

            return viewModel;
        }

        public async Task<int> Update(int id, GarmentShippingPaymentDispositionEMKLViewModel viewModel)
        {
            var model = MapToModel(viewModel);

            return await _repository.UpdateAsync(id, model);
        }
    }
}
