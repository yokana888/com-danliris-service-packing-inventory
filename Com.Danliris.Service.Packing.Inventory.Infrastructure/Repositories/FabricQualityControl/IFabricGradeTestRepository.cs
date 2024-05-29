//using Com.Danliris.Service.Packing.Inventory.Data;
//using Com.Danliris.Service.Packing.Inventory.Data.Models.FabricQualityControl;
using Com.Danliris.Service.Packing.Inventory.Data.Models.FabricQualityControl;
using Com.Moonlay.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Danliris.Service.Packing.Inventory.Infrastructure.Repositories.FabricQualityControl
{
    public interface IFabricGradeTestRepository : IRepository<FabricGradeTestModel>
    {
        Task<int> DeleteAsync(int id);
        IQueryable<FabricGradeTestModel> GetDbSet();
        Task<int> InsertAsync(FabricGradeTestModel model);
        IQueryable<FabricGradeTestModel> ReadAll();
        IQueryable<FabricGradeTestModel> ReadAllIgnoreQueryFilter();
        Task<FabricGradeTestModel> ReadByIdAsync(int id);
        Task<int> UpdateAsync(int id, FabricGradeTestModel model);

    }
}
