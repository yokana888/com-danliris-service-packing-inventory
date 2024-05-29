﻿using Com.Danliris.Service.Packing.Inventory.Data.Models.Garmentshipping.SalesExport;
using Com.Danliris.Service.Packing.Inventory.Data.Models.Garmentshipping.SalesExport;
using Com.Danliris.Service.Packing.Inventory.Infrastructure.IdentityProvider;
using Com.Moonlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Packing.Inventory.Infrastructure.Repositories.GarmentShipping.SalesExport
{
    public class GarmentShippingExportSalesNoteRepository : IGarmentShippingExportSalesNoteRepository
    {
        private const string UserAgent = "Repository";
        private readonly PackingInventoryDbContext _dbContext;
        private readonly IIdentityProvider _identityProvider;
        private readonly DbSet<GarmentShippingExportSalesNoteModel> _dbSet;
        private readonly DbSet<GarmentShippingExportSalesContractItemModel> _salesContractDbSet;

        public GarmentShippingExportSalesNoteRepository(PackingInventoryDbContext dbContext, IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<GarmentShippingExportSalesNoteModel>();
            _salesContractDbSet= dbContext.Set<GarmentShippingExportSalesContractItemModel>();
            _identityProvider = serviceProvider.GetService<IIdentityProvider>();
        }

        public Task<int> DeleteAsync(int id)
        {
            var model = _dbSet
                .Include(i => i.Items)
                .FirstOrDefault(s => s.Id == id);

            //var sc= _salesContractDbSet.FirstOrDefault(a => a.Id == model.ExportSalesContractId);

            //sc.SetIsUsed(false, _identityProvider.Username, UserAgent);
            model.FlagForDelete(_identityProvider.Username, UserAgent);

            foreach (var item in model.Items)
            {
                var sc = _salesContractDbSet.FirstOrDefault(a => a.Id == item.ExportSalesContractItemId);
                sc.SetRemainingQuantity(sc.RemainingQuantity + item.Quantity, _identityProvider.Username, UserAgent);

                item.FlagForDelete(_identityProvider.Username, UserAgent);
            }

            return _dbContext.SaveChangesAsync();
        }

        public Task<int> InsertAsync(GarmentShippingExportSalesNoteModel model)
        {
            model.FlagForCreate(_identityProvider.Username, UserAgent);

            //var sc = _salesContractDbSet.FirstOrDefault(a => a.Id == model.ExportSalesContractId);
            //sc.SetIsUsed(true, _identityProvider.Username, UserAgent);

            foreach (var item in model.Items)
            {
                var sc = _salesContractDbSet.FirstOrDefault(a => a.Id == item.ExportSalesContractItemId);
                sc.SetRemainingQuantity(sc.RemainingQuantity - item.Quantity, _identityProvider.Username, UserAgent);

                item.FlagForCreate(_identityProvider.Username, UserAgent);
            }

            _dbSet.Add(model);

            return _dbContext.SaveChangesAsync();
        }

        public IQueryable<GarmentShippingExportSalesNoteModel> ReadAll()
        {
            return _dbSet.AsNoTracking();
        }

        public Task<GarmentShippingExportSalesNoteModel> ReadByIdAsync(int id)
        {
            return _dbSet
                .Include(i => i.Items)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public Task<int> UpdateAsync(int id, GarmentShippingExportSalesNoteModel model)
        {
            var modelToUpdate = _dbSet
                .Include(i => i.Items)
                .FirstOrDefault(s => s.Id == id);

            modelToUpdate.SetDate(model.Date, _identityProvider.Username, UserAgent);
            modelToUpdate.SetTempo(model.Tempo, _identityProvider.Username, UserAgent);
            modelToUpdate.SetExpenditureNo(model.ExpenditureNo, _identityProvider.Username, UserAgent);
            modelToUpdate.SetDispositionNo(model.DispositionNo, _identityProvider.Username, UserAgent);
            modelToUpdate.SetUseVat(model.UseVat, _identityProvider.Username, UserAgent);
            modelToUpdate.SetRemark(model.Remark, _identityProvider.Username, UserAgent);
            modelToUpdate.SetPaymentType(model.PaymentType, _identityProvider.Username, UserAgent);
            modelToUpdate.SetPaymentType(model.PaymentType, _identityProvider.Username, UserAgent);
            modelToUpdate.SetRejectedReason(null, _identityProvider.Username, UserAgent);
            modelToUpdate.SetIsRejectedShipping(false, _identityProvider.Username, UserAgent);
            modelToUpdate.SetIsRejectedFinance(false, _identityProvider.Username, UserAgent);

            foreach (var itemToUpdate in modelToUpdate.Items)
            {
                var item = model.Items.FirstOrDefault(m => m.Id == itemToUpdate.Id);
                var sc = _salesContractDbSet.FirstOrDefault(a => a.Id == item.ExportSalesContractItemId);
                

                if (item != null)
                {

                    var qty = sc.RemainingQuantity + itemToUpdate.Quantity - item.Quantity;
                    sc.SetRemainingQuantity(qty, _identityProvider.Username, UserAgent);

                    itemToUpdate.SetProductId(item.ProductId, _identityProvider.Username, UserAgent);
                    itemToUpdate.SetProductCode(item.ProductCode, _identityProvider.Username, UserAgent);
                    itemToUpdate.SetProductName(item.ProductName, _identityProvider.Username, UserAgent);
                    itemToUpdate.SetQuantity(item.Quantity, _identityProvider.Username, UserAgent);
                    itemToUpdate.SetUomId(item.UomId, _identityProvider.Username, UserAgent);
                    itemToUpdate.SetUomUnit(item.UomUnit, _identityProvider.Username, UserAgent);
                    itemToUpdate.SetPrice(item.Price, _identityProvider.Username, UserAgent);
                    itemToUpdate.SetPackageUomUnit(item.PackageUomUnit, _identityProvider.Username, UserAgent);
                    itemToUpdate.SetPackageUomId(item.UomId, _identityProvider.Username, UserAgent);
                    itemToUpdate.SetPackageQuantity(item.PackageQuantity, _identityProvider.Username, UserAgent);
                    itemToUpdate.SetRemark(item.Remark, _identityProvider.Username, UserAgent);

                }
                else
                {
                    itemToUpdate.FlagForDelete(_identityProvider.Username, UserAgent);

                    sc.SetRemainingQuantity(sc.RemainingQuantity + itemToUpdate.Quantity, _identityProvider.Username, UserAgent);
                }
            }

            foreach (var item in model.Items.Where(w => w.Id == 0))
            {
                var sc = _salesContractDbSet.FirstOrDefault(a => a.Id == item.ExportSalesContractItemId);
                sc.SetRemainingQuantity(sc.RemainingQuantity - item.Quantity, _identityProvider.Username, UserAgent);

                modelToUpdate.Items.Add(item);
            }

            return _dbContext.SaveChangesAsync();
        }

        public Task<int> ApproveFinanceAsync(int id)
        {
            var modelToUpdate = _dbSet
                .FirstOrDefault(s => s.Id == id);

            modelToUpdate.SetApproveFinanceDate(DateTimeOffset.Now, _identityProvider.Username, UserAgent);
            modelToUpdate.SetIsApproveFinance(true, _identityProvider.Username, UserAgent);
            modelToUpdate.SetApproveFinanceBy(_identityProvider.Username, _identityProvider.Username, UserAgent);

            return _dbContext.SaveChangesAsync();
        }

        public Task<int> ApproveShippingAsync(int id)
        {
            var modelToUpdate = _dbSet
                .FirstOrDefault(s => s.Id == id);

            modelToUpdate.SetApproveShippingDate(DateTimeOffset.Now, _identityProvider.Username, UserAgent);
            modelToUpdate.SetIsApproveShipping(true, _identityProvider.Username, UserAgent);
            modelToUpdate.SetApproveShippingBy(_identityProvider.Username, _identityProvider.Username, UserAgent);

            return _dbContext.SaveChangesAsync();
        }

        public Task<int> RejectShippingAsync(int id, GarmentShippingExportSalesNoteModel model)
        {
            var modelToUpdate = _dbSet
                .FirstOrDefault(s => s.Id == id);

            modelToUpdate.SetIsRejectedShipping(true, _identityProvider.Username, UserAgent);
            modelToUpdate.SetRejectedReason(model.RejectedReason, _identityProvider.Username, UserAgent);

            return _dbContext.SaveChangesAsync();
        }

        public Task<int> RejectFinanceAsync(int id, GarmentShippingExportSalesNoteModel model)
        {
            var modelToUpdate = _dbSet
                .FirstOrDefault(s => s.Id == id);

            modelToUpdate.SetIsRejectedFinance(true, _identityProvider.Username, UserAgent);
            modelToUpdate.SetRejectedReason(model.RejectedReason, _identityProvider.Username, UserAgent);
            modelToUpdate.SetIsApproveShipping(false, _identityProvider.Username, UserAgent);

            return _dbContext.SaveChangesAsync();
        }
    }
}
