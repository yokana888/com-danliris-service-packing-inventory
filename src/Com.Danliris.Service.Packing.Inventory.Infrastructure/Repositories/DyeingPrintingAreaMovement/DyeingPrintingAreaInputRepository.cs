﻿using Com.Danliris.Service.Packing.Inventory.Data.Models.DyeingPrintingAreaMovement;
using Com.Danliris.Service.Packing.Inventory.Infrastructure.IdentityProvider;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Com.Moonlay.Models;
using Com.Danliris.Service.Packing.Inventory.Infrastructure.Utilities;
using Newtonsoft.Json;

namespace Com.Danliris.Service.Packing.Inventory.Infrastructure.Repositories.DyeingPrintingAreaMovement
{
    public class DyeingPrintingAreaInputRepository : IDyeingPrintingAreaInputRepository
    {
        private const string UserAgent = "Repository";
        private readonly PackingInventoryDbContext _dbContext;
        private readonly IIdentityProvider _identityProvider;
        private readonly DbSet<DyeingPrintingAreaInputModel> _dbSet;
        private readonly IDyeingPrintingAreaOutputProductionOrderRepository _outputSPPRepository;
        private readonly IDyeingPrintingAreaInputProductionOrderRepository _SPPRepository;

        public DyeingPrintingAreaInputRepository(PackingInventoryDbContext dbContext, IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<DyeingPrintingAreaInputModel>();
            _outputSPPRepository = serviceProvider.GetService<IDyeingPrintingAreaOutputProductionOrderRepository>();
            _identityProvider = serviceProvider.GetService<IIdentityProvider>();
            _SPPRepository = serviceProvider.GetService<IDyeingPrintingAreaInputProductionOrderRepository>();
        }

        public Task<int> DeleteAsync(int id)
        {
            var model = _dbSet.Include(s => s.DyeingPrintingAreaInputProductionOrders).FirstOrDefault(s => s.Id == id);

            model.FlagForDelete(_identityProvider.Username, UserAgent);
            foreach (var item in model.DyeingPrintingAreaInputProductionOrders)
            {
                item.FlagForDelete(_identityProvider.Username, UserAgent);
            }

            _dbSet.Update(model);
            return _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAvalTransformationArea(DyeingPrintingAreaInputModel model)
        {
            int result = 0;
            foreach (var item in model.DyeingPrintingAreaInputProductionOrders.Where(s => !s.HasOutputDocument))
            {
                item.FlagForDelete(_identityProvider.Username, UserAgent);
                result += await _SPPRepository.UpdateFromOutputAsync(item.DyeingPrintingAreaOutputProductionOrderId, false);
            }
            model.FlagForDelete(_identityProvider.Username, UserAgent);
            _dbSet.Update(model);

            result += await _dbContext.SaveChangesAsync();
            return result;
        }

        public Task<int> DeleteIMArea(DyeingPrintingAreaInputModel model)
        {
            foreach (var item in model.DyeingPrintingAreaInputProductionOrders.Where(s => !s.HasOutputDocument))
            {
                item.FlagForDelete(_identityProvider.Username, UserAgent);
            }

            //model.FlagForDelete(_identityProvider.Username, UserAgent);
            _dbSet.Update(model);
            return _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteShippingArea(DyeingPrintingAreaInputModel model)
        {
            int result = 0;
            if(model.ShippingType == DyeingPrintingArea.ZONAGUDANG)
            {
                result += await _outputSPPRepository.UpdateFromInputAsync(model.DyeingPrintingAreaInputProductionOrders.Select(s => s.DyeingPrintingAreaOutputProductionOrderId).ToList(), false, null);
            }
            
            foreach (var item in model.DyeingPrintingAreaInputProductionOrders.Where(s => !s.HasOutputDocument))
            {
                item.FlagForDelete(_identityProvider.Username, UserAgent);
                if(model.ShippingType == DyeingPrintingArea.ZONAGUDANG)
                {
                    var previousOutputData = await _outputSPPRepository.ReadByIdAsync(item.DyeingPrintingAreaOutputProductionOrderId);
                    result += await _SPPRepository.UpdateFromNextAreaInputAsync(previousOutputData.DyeingPrintingAreaInputProductionOrderId, item.Balance * -1, item.PackagingQty * -1);
                }
                
            }

            _dbSet.Update(model);

            result += await _dbContext.SaveChangesAsync();
            return result;
        }

        public async Task<int> DeleteTransitArea(DyeingPrintingAreaInputModel model)
        {
            int result = 0;
            result += await _outputSPPRepository.UpdateFromInputAsync(model.DyeingPrintingAreaInputProductionOrders.Select(s => s.DyeingPrintingAreaOutputProductionOrderId).ToList(), false, null);
            foreach (var item in model.DyeingPrintingAreaInputProductionOrders.Where(s => !s.HasOutputDocument))
            {
                item.FlagForDelete(_identityProvider.Username, UserAgent);
                var previousOutputData = await _outputSPPRepository.ReadByIdAsync(item.DyeingPrintingAreaOutputProductionOrderId);
                if (previousOutputData.Area == DyeingPrintingArea.PACKING)
                {
                    //var outputData = await _outputProductionOrderRepository.ReadByIdAsync(item.Id);
                    var packingData = JsonConvert.DeserializeObject<List<PackingData>>(previousOutputData.PrevSppInJson);
                    foreach(var pack in packingData)
                    {
                        pack.Balance *= -1;
                    }
                    result += await _SPPRepository.UpdateFromNextAreaInputPackingAsync(packingData);
                }
                else
                {

                    result += await _SPPRepository.UpdateFromNextAreaInputAsync(previousOutputData.DyeingPrintingAreaInputProductionOrderId, item.Balance * -1, item.PackagingQty * -1);
                }
                //result += await _SPPRepository.UpdateFromNextAreaInputAsync(previousOutputData.DyeingPrintingAreaInputProductionOrderId, item.Balance * -1, item.PackagingQty * -1);
            }

            //model.FlagForDelete(_identityProvider.Username, UserAgent);
            _dbSet.Update(model);

            result += await _dbContext.SaveChangesAsync();
            return result;
        }

        public IQueryable<DyeingPrintingAreaInputModel> GetDbSet()
        {
            return _dbSet;
        }

        public Task<int> InsertAsync(DyeingPrintingAreaInputModel model)
        {
            model.FlagForCreate(_identityProvider.Username, UserAgent);
            foreach (var item in model.DyeingPrintingAreaInputProductionOrders)
            {
                item.FlagForCreate(_identityProvider.Username, UserAgent);
            }

            _dbSet.Add(model);

            return _dbContext.SaveChangesAsync();
        }

        public IQueryable<DyeingPrintingAreaInputModel> ReadAll()
        {
            return _dbSet.Include(s => s.DyeingPrintingAreaInputProductionOrders).AsNoTracking();
        }

        public IQueryable<DyeingPrintingAreaInputModel> ReadAllIgnoreQueryFilter()
        {
            return _dbSet.Include(s => s.DyeingPrintingAreaInputProductionOrders).IgnoreQueryFilters().AsNoTracking();
        }

        public Task<DyeingPrintingAreaInputModel> ReadByIdAsync(int id)
        {
            return _dbSet.Include(s => s.DyeingPrintingAreaInputProductionOrders).FirstOrDefaultAsync(s => s.Id == id);
        }

        public Task<int> RestoreAvalTransformation(List<AvalData> avalData)
        {
            foreach (var item in avalData)
            {
                var model = _dbSet.FirstOrDefault(s => s.Id == item.Id);
                if (model != null)
                {
                    var newAvalQuantity = model.TotalAvalQuantity + item.AvalQuantity;
                    var newWeightQuantity = model.TotalAvalWeight + item.AvalQuantityKg;

                    model.SetTotalAvalQuantity(newAvalQuantity, _identityProvider.Username, UserAgent);
                    model.SetTotalAvalWeight(newWeightQuantity, _identityProvider.Username, UserAgent);
                }

            }

            return _dbContext.SaveChangesAsync();
        }

        public Task<int> UpdateAsync(int id, DyeingPrintingAreaInputModel model)
        {
            var modelToUpdate = _dbSet.Include(s => s.DyeingPrintingAreaInputProductionOrders).FirstOrDefault(s => s.Id == id);
            modelToUpdate.SetArea(model.Area, _identityProvider.Username, UserAgent);
            modelToUpdate.SetBonNo(model.BonNo, _identityProvider.Username, UserAgent);
            modelToUpdate.SetDate(model.Date, _identityProvider.Username, UserAgent);
            modelToUpdate.SetShift(model.Shift, _identityProvider.Username, UserAgent);
            modelToUpdate.SetGroup(model.Group, _identityProvider.Username, UserAgent);
            modelToUpdate.SetTotalAvalQuantity(model.TotalAvalQuantity, _identityProvider.Username, UserAgent);
            modelToUpdate.SetTotalAvalWeight(model.TotalAvalWeight, _identityProvider.Username, UserAgent);

            foreach (var item in modelToUpdate.DyeingPrintingAreaInputProductionOrders)
            {
                var localItem = model.DyeingPrintingAreaInputProductionOrders.FirstOrDefault(s => s.Id == item.Id);

                if (localItem == null)
                {
                    item.FlagForDelete(_identityProvider.Username, UserAgent);
                }
                else
                {
                    item.SetArea(localItem.Area, _identityProvider.Username, UserAgent);
                    item.SetBalance(localItem.Balance, _identityProvider.Username, UserAgent);
                    item.SetBuyer(localItem.BuyerId, localItem.Buyer, _identityProvider.Username, UserAgent);
                    item.SetCartNo(localItem.CartNo, _identityProvider.Username, UserAgent);
                    item.SetColor(localItem.Color, _identityProvider.Username, UserAgent);
                    item.SetConstruction(localItem.Construction, _identityProvider.Username, UserAgent);
                    item.SetGrade(localItem.Grade, _identityProvider.Username, UserAgent);
                    item.SetHasOutputDocument(localItem.HasOutputDocument, _identityProvider.Username, UserAgent);
                    item.SetMotif(localItem.Motif, _identityProvider.Username, UserAgent);
                    item.SetProductionOrder(localItem.ProductionOrderId, localItem.ProductionOrderNo, localItem.ProductionOrderType, localItem.ProductionOrderOrderQuantity, _identityProvider.Username, UserAgent);
                    item.SetRemark(localItem.Remark, _identityProvider.Username, UserAgent);
                    item.SetStatus(localItem.Status, _identityProvider.Username, UserAgent);
                    item.SetUnit(localItem.Unit, _identityProvider.Username, UserAgent);
                    item.SetUomUnit(localItem.UomUnit, _identityProvider.Username, UserAgent);
                    item.SetIsChecked(localItem.IsChecked, _identityProvider.Username, UserAgent);
                    item.SetPackingInstruction(localItem.PackingInstruction, _identityProvider.Username, UserAgent);
                    item.SetDeliveryOrderSales(localItem.DeliveryOrderSalesId, localItem.DeliveryOrderSalesNo, _identityProvider.Username, UserAgent);
                    item.SetMaterial(localItem.MaterialId, localItem.MaterialName, _identityProvider.Username, UserAgent);
                    item.SetMaterialConstruction(localItem.MaterialConstructionId, localItem.MaterialConstructionName, _identityProvider.Username, UserAgent);
                    item.SetMaterialWidth(localItem.MaterialWidth, _identityProvider.Username, UserAgent);
                    item.SetMachine(localItem.Machine, _identityProvider.Username, UserAgent);
                    item.SetPackagingLength(localItem.PackagingLength, _identityProvider.Username, UserAgent);
                }
            }

            foreach (var item in model.DyeingPrintingAreaInputProductionOrders.Where(s => s.Id == 0))
            {
                item.FlagForCreate(_identityProvider.Username, UserAgent);
                modelToUpdate.DyeingPrintingAreaInputProductionOrders.Add(item);
            }

            return _dbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateAvalTransformationArea(int id, DyeingPrintingAreaInputModel model, DyeingPrintingAreaInputModel dbModel)
        {
            int result = 0;
            dbModel.SetDate(model.Date, _identityProvider.Username, UserAgent);
            dbModel.SetShift(model.Shift, _identityProvider.Username, UserAgent);
            dbModel.SetGroup(model.Group, _identityProvider.Username, UserAgent);
            dbModel.SetIsTransformedAval(model.IsTransformedAval, _identityProvider.Username, UserAgent);
            dbModel.SetTotalAvalQuantity(model.TotalAvalQuantity, _identityProvider.Username, UserAgent);
            dbModel.SetTotalAvalWeight(model.TotalAvalWeight, _identityProvider.Username, UserAgent);

            foreach (var item in dbModel.DyeingPrintingAreaInputProductionOrders.Where(s => !s.HasOutputDocument))
            {
                var localItem = model.DyeingPrintingAreaInputProductionOrders.FirstOrDefault(s => s.Id == item.Id);

                if (localItem == null)
                {
                    item.FlagForDelete(_identityProvider.Username, UserAgent);
                    result += await _SPPRepository.UpdateFromOutputAsync(item.DyeingPrintingAreaOutputProductionOrderId, false);
                }
            }

            result += await _dbContext.SaveChangesAsync();
            return result;
        }

        public async Task<Tuple<int, List<AvalData>>> UpdateAvalTransformationFromOut(string avalType, double avalQuantity, double weightQuantity)
        {
            var data = _dbSet.Where(s => s.Area == DyeingPrintingArea.GUDANGAVAL && s.AvalType == avalType && s.IsTransformedAval && (s.TotalAvalQuantity != 0 || s.TotalAvalWeight != 0)).OrderBy(s => s.Date).ToList();

            int index = 0;
            var avalData = new List<AvalData>();
            while (avalQuantity > 0 || weightQuantity > 0)
            {
                var item = data.ElementAtOrDefault(index);

                if (item != null)
                {
                    var previousAval = new AvalData()
                    {
                        Id = item.Id
                    };

                    double diffQuantity = 0;
                    double diffWeight = 0;

                    if (avalQuantity > 0)
                    {
                        var tempTotalQuantity = item.TotalAvalQuantity - avalQuantity;
                        if (tempTotalQuantity < 0)
                        {
                            avalQuantity = tempTotalQuantity * -1;
                            diffQuantity = item.TotalAvalQuantity;
                            item.SetTotalAvalQuantity(0, _identityProvider.Username, UserAgent);
                        }
                        else
                        {
                            diffQuantity = avalQuantity;
                            item.SetTotalAvalQuantity(tempTotalQuantity, _identityProvider.Username, UserAgent);
                            avalQuantity = 0;
                        }
                    }

                    if (weightQuantity > 0)
                    {
                        var tempTotalWeight = item.TotalAvalWeight - weightQuantity;
                        if (tempTotalWeight < 0)
                        {
                            weightQuantity = tempTotalWeight * -1;
                            diffWeight = item.TotalAvalWeight;
                            item.SetTotalAvalWeight(0, _identityProvider.Username, UserAgent);

                        }
                        else
                        {
                            diffWeight = weightQuantity;
                            item.SetTotalAvalWeight(tempTotalWeight, _identityProvider.Username, UserAgent);
                            weightQuantity = 0;
                        }
                    }

                    previousAval.AvalQuantity = diffQuantity;
                    previousAval.AvalQuantityKg = diffWeight;

                    avalData.Add(previousAval);

                    index++;
                }
                else
                {
                    break;
                }
            }

            int result = await _dbContext.SaveChangesAsync();

            return new Tuple<int, List<AvalData>>(result, avalData);
        }

        public Task<int> UpdateHeaderAvalTransform(DyeingPrintingAreaInputModel model, double avalQuantity, double weightQuantity)
        {
            var newAvalQuantity = model.TotalAvalQuantity + avalQuantity;
            var newWeightQuantity = model.TotalAvalWeight + weightQuantity;

            model.SetTotalAvalQuantity(newAvalQuantity, _identityProvider.Username, UserAgent);
            model.SetTotalAvalWeight(newWeightQuantity, _identityProvider.Username, UserAgent);

            _dbSet.Update(model);

            return _dbContext.SaveChangesAsync();
        }

        public Task<int> UpdateIMArea(int id, DyeingPrintingAreaInputModel model, DyeingPrintingAreaInputModel modelToUpdate)
        {
            modelToUpdate.SetDate(model.Date, _identityProvider.Username, UserAgent);
            modelToUpdate.SetShift(model.Shift, _identityProvider.Username, UserAgent);
            modelToUpdate.SetGroup(model.Group, _identityProvider.Username, UserAgent);

            foreach (var item in modelToUpdate.DyeingPrintingAreaInputProductionOrders.Where(s => !s.HasOutputDocument))
            {
                var localItem = model.DyeingPrintingAreaInputProductionOrders.FirstOrDefault(s => s.Id == item.Id);

                if (localItem == null)
                {
                    item.FlagForDelete(_identityProvider.Username, UserAgent);

                }
                else
                {
                    item.SetCartNo(localItem.CartNo, _identityProvider.Username, UserAgent);
                    var diffBalance = localItem.InputQuantity - item.InputQuantity;

                    var newBalanceRemains = item.BalanceRemains + diffBalance;
                    var newBalance = item.Balance + diffBalance;
                    
                    if (newBalanceRemains <= 0)
                    {
                        item.SetHasOutputDocument(true, _identityProvider.Username, UserAgent);
                    }
                    else
                    {
                        item.SetHasOutputDocument(false, _identityProvider.Username, UserAgent);
                    }
                    item.SetBalanceRemains(newBalanceRemains, _identityProvider.Username, UserAgent);
                    item.SetBalance(newBalance, _identityProvider.Username, UserAgent);
                    item.SetInputQuantity(localItem.InputQuantity, _identityProvider.Username, UserAgent);
                }
            }

            foreach (var item in model.DyeingPrintingAreaInputProductionOrders.Where(s => s.Id == 0))
            {
                item.FlagForCreate(_identityProvider.Username, UserAgent);
                modelToUpdate.DyeingPrintingAreaInputProductionOrders.Add(item);
            }

            return _dbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateShippingArea(int id, DyeingPrintingAreaInputModel model, DyeingPrintingAreaInputModel dbModel)
        {
            int result = 0;
            dbModel.SetDate(model.Date, _identityProvider.Username, UserAgent);
            dbModel.SetShift(model.Shift, _identityProvider.Username, UserAgent);
            dbModel.SetGroup(model.Group, _identityProvider.Username, UserAgent);

            foreach (var item in dbModel.DyeingPrintingAreaInputProductionOrders.Where(s => !s.HasOutputDocument))
            {
                var localItem = model.DyeingPrintingAreaInputProductionOrders.FirstOrDefault(s => s.Id == item.Id);

                if (localItem == null)
                {
                    item.FlagForDelete(_identityProvider.Username, UserAgent);

                    if(model.ShippingType == DyeingPrintingArea.ZONAGUDANG)
                    {
                        result += await _outputSPPRepository.UpdateFromInputNextAreaFlagAsync(item.DyeingPrintingAreaOutputProductionOrderId, false, null);
                        var previousOutputData = await _outputSPPRepository.ReadByIdAsync(item.DyeingPrintingAreaOutputProductionOrderId);
                        result += await _SPPRepository.UpdateFromNextAreaInputAsync(previousOutputData.DyeingPrintingAreaInputProductionOrderId, item.Balance * -1, item.PackagingQty * -1);
                    }

                }
                else
                {
                    var diffBalance = localItem.InputQuantity - item.InputQuantity;
                    var diffQtyPacking = localItem.InputPackagingQty - item.InputPackagingQty;

                    var newBalanceRemains = item.BalanceRemains + diffBalance;
                    var newBalance = item.Balance + diffBalance;

                    var newPackagingQty = item.PackagingQty + diffQtyPacking;

                    item.SetGrade(localItem.Grade, _identityProvider.Username, UserAgent);
                    item.SetPackagingType(localItem.PackagingType, _identityProvider.Username, UserAgent);
                    item.SetPackagingQty(newPackagingQty, _identityProvider.Username, UserAgent);
                    item.SetPackagingUnit(localItem.PackagingUnit, _identityProvider.Username, UserAgent);
                    item.SetPackagingLength(localItem.PackagingLength, _identityProvider.Username, UserAgent);
                    item.SetInputPackagingQty(localItem.InputPackagingQty, _identityProvider.Username, UserAgent);
                    item.SetBalance(newBalance, _identityProvider.Username, UserAgent);
                    item.SetInputQuantity(localItem.InputQuantity, _identityProvider.Username, UserAgent);
                    item.SetBalanceRemains(newBalanceRemains, _identityProvider.Username, UserAgent);
                }
            }

            if(model.ShippingType == DyeingPrintingArea.RETURBARANG)
            {
                foreach (var item in model.DyeingPrintingAreaInputProductionOrders.Where(s => s.Id == 0))
                {
                    item.FlagForCreate(_identityProvider.Username, UserAgent);
                    dbModel.DyeingPrintingAreaInputProductionOrders.Add(item);
                }
            }
            

            //foreach (var item in model.DyeingPrintingAreaInputProductionOrders.Where(s => s.Id == 0))
            //{
            //    item.FlagForCreate(_identityProvider.Username, UserAgent);
            //    dbModel.DyeingPrintingAreaInputProductionOrders.Add(item);
            //    result += await _outputSPPRepository.UpdateFromInputNextAreaFlagAsync(item.DyeingPrintingAreaOutputProductionOrderId, true);
            //    var previousOutputData = await _outputSPPRepository.ReadByIdAsync(item.DyeingPrintingAreaOutputProductionOrderId);
            //    result += await _SPPRepository.UpdateFromNextAreaInputAsync(previousOutputData.DyeingPrintingAreaInputProductionOrderId, item.Balance);
            //}

            result += await _dbContext.SaveChangesAsync();
            return result;
        }

        public async Task<int> UpdateTransitArea(int id, DyeingPrintingAreaInputModel model, DyeingPrintingAreaInputModel dbModel)
        {
            int result = 0;
            dbModel.SetDate(model.Date, _identityProvider.Username, UserAgent);
            dbModel.SetShift(model.Shift, _identityProvider.Username, UserAgent);
            dbModel.SetGroup(model.Group, _identityProvider.Username, UserAgent);

            foreach (var item in dbModel.DyeingPrintingAreaInputProductionOrders.Where(s => !s.HasOutputDocument))
            {
                var localItem = model.DyeingPrintingAreaInputProductionOrders.FirstOrDefault(s => s.Id == item.Id);

                if (localItem == null)
                {
                    item.FlagForDelete(_identityProvider.Username, UserAgent);
                    result += await _outputSPPRepository.UpdateFromInputNextAreaFlagAsync(item.DyeingPrintingAreaOutputProductionOrderId, false, null);
                    var previousOutputData = await _outputSPPRepository.ReadByIdAsync(item.DyeingPrintingAreaOutputProductionOrderId);

                    if (previousOutputData.Area == DyeingPrintingArea.PACKING)
                    {
                        //var outputData = await _outputProductionOrderRepository.ReadByIdAsync(item.Id);
                        var packingData = JsonConvert.DeserializeObject<List<PackingData>>(previousOutputData.PrevSppInJson);
                        foreach (var pack in packingData)
                        {
                            pack.Balance *= -1;
                        }
                        result += await _SPPRepository.UpdateFromNextAreaInputPackingAsync(packingData);
                    }
                    else
                    {

                        result += await _SPPRepository.UpdateFromNextAreaInputAsync(previousOutputData.DyeingPrintingAreaInputProductionOrderId, item.Balance * -1, item.PackagingQty * -1);
                    }
                    //result += await _SPPRepository.UpdateFromNextAreaInputAsync(previousOutputData.DyeingPrintingAreaInputProductionOrderId, item.Balance * -1, item.PackagingQty * -1);
                }
            }

            //foreach (var item in model.DyeingPrintingAreaInputProductionOrders.Where(s => s.Id == 0))
            //{
            //    item.FlagForCreate(_identityProvider.Username, UserAgent);
            //    dbModel.DyeingPrintingAreaInputProductionOrders.Add(item);
            //    result += await _outputSPPRepository.UpdateFromInputNextAreaFlagAsync(item.DyeingPrintingAreaOutputProductionOrderId, true);
            //    var previousOutputData = await _outputSPPRepository.ReadByIdAsync(item.DyeingPrintingAreaOutputProductionOrderId);
            //    result += await _SPPRepository.UpdateFromNextAreaInputAsync(previousOutputData.DyeingPrintingAreaInputProductionOrderId, item.Balance);
            //}

            result += await _dbContext.SaveChangesAsync();
            return result;
        }
    }
}
