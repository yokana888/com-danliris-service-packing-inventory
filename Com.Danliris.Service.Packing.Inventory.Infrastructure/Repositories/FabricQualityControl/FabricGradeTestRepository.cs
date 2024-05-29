using Com.Danliris.Service.Packing.Inventory.Data.Models.FabricQualityControl;
using Com.Danliris.Service.Packing.Inventory.Infrastructure.IdentityProvider;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Com.Moonlay.Models;
using Com.Moonlay.Data;
using System.Linq.Expressions;

namespace Com.Danliris.Service.Packing.Inventory.Infrastructure.Repositories.FabricQualityControl
{
    public class FabricGradeTestRepository : IFabricGradeTestRepository
    {
        private const string UserAgent = "Repository";
        private readonly PackingInventoryDbContext _dbContext;
        private readonly DbSet<FabricGradeTestModel> _fabricGradeTestDbSet;
        private readonly DbSet<CriteriaModel> _criteriaDbSet;
        private readonly IIdentityProvider _identityProvider;

        public FabricGradeTestRepository(PackingInventoryDbContext dbContext, IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _fabricGradeTestDbSet = dbContext.Set<FabricGradeTestModel>();
            _criteriaDbSet = dbContext.Set<CriteriaModel>();
            _identityProvider = serviceProvider.GetService<IIdentityProvider>();
        }

        public Task<int> DeleteAsync(int id)
        {
            var model = _fabricGradeTestDbSet.Include(s => s.Criteria).FirstOrDefault(entity => entity.Id == id);
            model.FlagForDelete(_identityProvider.Username, UserAgent);

            _fabricGradeTestDbSet.Update(model);
            return _dbContext.SaveChangesAsync();
        }

        public IQueryable<FabricGradeTestModel> GetDbSet()
        {
            return _fabricGradeTestDbSet;
        }

        public Task<int> InsertAsync(FabricGradeTestModel model)
        {
            model.FlagForCreate(_identityProvider.Username, UserAgent);

            _fabricGradeTestDbSet.Add(model);
            return _dbContext.SaveChangesAsync();
        }

        public IQueryable<FabricGradeTestModel> ReadAll()
        {
            return _fabricGradeTestDbSet.Include(s => s.Criteria).AsNoTracking();
        }

        public IQueryable<FabricGradeTestModel> ReadAllIgnoreQueryFilter()
        {
            return _fabricGradeTestDbSet.Include(s => s.Criteria).IgnoreQueryFilters().AsNoTracking();
        }

        public Task<FabricGradeTestModel> ReadByIdAsync(int id)
        {
            return _fabricGradeTestDbSet.Include(s => s.Criteria).FirstOrDefaultAsync(s => s.Id == id);
        }

        public Task<int> UpdateAsync(int id, FabricGradeTestModel model)
        {
            var modelToUpdate = _fabricGradeTestDbSet.Include(s => s.Criteria).FirstOrDefault(entity => entity.Id == id);
            modelToUpdate.SetAvalALength(model.AvalALength, _identityProvider.Username, UserAgent);
            modelToUpdate.SetAvalBLength(model.AvalBLength, _identityProvider.Username, UserAgent);
            modelToUpdate.SetAvalConnectionLength(model.AvalConnectionLength, _identityProvider.Username, UserAgent);
            modelToUpdate.SetFabricGradeTest(model.FabricGradeTest, _identityProvider.Username, UserAgent);
            modelToUpdate.SetFinalArea(model.FinalArea, _identityProvider.Username, UserAgent);
            modelToUpdate.SetFinalGradeTest(model.FinalGradeTest, _identityProvider.Username, UserAgent);
            modelToUpdate.SetFinalLength(model.FinalLength, _identityProvider.Username, UserAgent);
            modelToUpdate.SetFinalScore(model.FinalScore, _identityProvider.Username, UserAgent);
            modelToUpdate.SetGrade(model.Grade, _identityProvider.Username, UserAgent);
            modelToUpdate.SetInitLength(model.InitLength, _identityProvider.Username, UserAgent);
            modelToUpdate.SetItemIndex(model.ItemIndex, _identityProvider.Username, UserAgent);
            modelToUpdate.SetPcsNo(model.PcsNo, _identityProvider.Username, UserAgent);
            modelToUpdate.SetPointLimit(model.PointLimit, _identityProvider.Username, UserAgent);
            modelToUpdate.SetPointSystem(model.PointSystem, _identityProvider.Username, UserAgent);
            modelToUpdate.SetSampleLength(model.SampleLength, _identityProvider.Username, UserAgent);
            modelToUpdate.SetScore(model.Score, _identityProvider.Username, UserAgent);
            modelToUpdate.SetType(model.Type, _identityProvider.Username, UserAgent);
            modelToUpdate.SetWidth(model.Width, _identityProvider.Username, UserAgent);

            foreach (var criteria in modelToUpdate.Criteria)
            {
                var localCriteria = model.Criteria.FirstOrDefault(s => s.Id == criteria.Id);

                if (localCriteria != null)
                {
                    criteria.SetCode(localCriteria.Code);
                    criteria.SetGroup(localCriteria.Group);
                    criteria.SetIndex(localCriteria.Index);
                    criteria.SetName(localCriteria.Name);
                    criteria.SetScoreA(localCriteria.ScoreA);
                    criteria.SetScoreB(localCriteria.ScoreB);
                    criteria.SetScoreC(localCriteria.ScoreC);
                    criteria.SetScoreD(localCriteria.ScoreD);
                }
                else
                {
                    _criteriaDbSet.Remove(criteria);
                }
            }

            foreach (var newCriteria in model.Criteria.Where(s => s.Id == 0))
            {
                modelToUpdate.Criteria.Add(newCriteria);
            }

            return _dbContext.SaveChangesAsync();
        }

        int IRepository<FabricGradeTestModel>.Create(FabricGradeTestModel entity)
        {
            throw new NotImplementedException();
        }

        Task<int> IRepository<FabricGradeTestModel>.CreateAsync(FabricGradeTestModel entity)
        {
            throw new NotImplementedException();
        }

        int IRepository<FabricGradeTestModel>.Delete(FabricGradeTestModel entity)
        {
            throw new NotImplementedException();
        }

        Task<int> IFabricGradeTestRepository.DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<int> IRepository<FabricGradeTestModel>.DeleteAsync(FabricGradeTestModel entity)
        {
            throw new NotImplementedException();
        }

        FabricGradeTestModel IRepository<FabricGradeTestModel>.Get(params object[] keys)
        {
            throw new NotImplementedException();
        }

        Task<FabricGradeTestModel> IRepository<FabricGradeTestModel>.GetAsync(params object[] keys)
        {
            throw new NotImplementedException();
        }

        IQueryable<FabricGradeTestModel> IFabricGradeTestRepository.GetDbSet()
        {
            throw new NotImplementedException();
        }

        Task<int> IFabricGradeTestRepository.InsertAsync(FabricGradeTestModel model)
        {
            throw new NotImplementedException();
        }

        bool IRepository<FabricGradeTestModel>.IsExist(params object[] keys)
        {
            throw new NotImplementedException();
        }

        Task<bool> IRepository<FabricGradeTestModel>.IsExistAsync(params object[] keys)
        {
            throw new NotImplementedException();
        }

        IEnumerable<FabricGradeTestModel> IRepository<FabricGradeTestModel>.Query(Expression<Func<FabricGradeTestModel, bool>> filter)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<FabricGradeTestModel>> IRepository<FabricGradeTestModel>.QueryAsync(Expression<Func<FabricGradeTestModel, bool>> filter)
        {
            throw new NotImplementedException();
        }

        IQueryable<FabricGradeTestModel> IFabricGradeTestRepository.ReadAll()
        {
            throw new NotImplementedException();
        }

        IQueryable<FabricGradeTestModel> IFabricGradeTestRepository.ReadAllIgnoreQueryFilter()
        {
            throw new NotImplementedException();
        }

        Task<FabricGradeTestModel> IFabricGradeTestRepository.ReadByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        int IRepository<FabricGradeTestModel>.Update(FabricGradeTestModel entity)
        {
            throw new NotImplementedException();
        }

        Task<int> IFabricGradeTestRepository.UpdateAsync(int id, FabricGradeTestModel model)
        {
            throw new NotImplementedException();
        }

        Task<int> IRepository<FabricGradeTestModel>.UpdateAsync(FabricGradeTestModel entity)
        {
            throw new NotImplementedException();
        }
    }
}
