using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESchool.Data.Repositories;
using ESchool.Domain.Entities.Examinations;
using ESchool.Domain.Enums;

namespace ESchool.Services.Examinations
{
    public class QTagService : IQTagService
    {
        private readonly IRepository<QTag> _qtagRepository;

        public QTagService(IRepository<QTag> qtagRepository)
        {
            _qtagRepository = qtagRepository;
        }

        public async Task<QTag> FindAsync(int id)
        {
            return await _qtagRepository.FindAsync(id);
        }

        public async Task<IEnumerable<QTag>> GetListAsync()
        {
            return await _qtagRepository.QueryNoTracking
                .Sort(o => o.OrderBy(t => t.Name))
                .GetListAsync();
        }

        public async Task<ErrorCode> CreateAsync(QTag entity)
        {
            var duplicateEntity = await FindAsync(entity.Name);

            if (duplicateEntity != null)
            {
                return ErrorCode.DuplicateEntity;
            }

            await _qtagRepository.CreateCommitAsync(entity);

            return ErrorCode.Success;
        }

        public async Task<ErrorCode> UpdateAsync(QTag entity)
        {
            var updatedEntity = await FindAsync(entity.Id);

            if (updatedEntity == null)
            {
                return ErrorCode.NotFound;
            }

            var duplicateEntity = await FindAsync(entity.Name);

            if (duplicateEntity != null && duplicateEntity.Id != entity.Id)
            {
                return ErrorCode.DuplicateEntity;
            }

            updatedEntity.Name = entity.Name;
            updatedEntity.Description = entity.Description;
            await _qtagRepository.UpdateCommitAsync(entity);

            return ErrorCode.Success;
        }

        public async Task<ErrorCode> DeleteAsync(int id)
        {
            var entity = await FindAsync(id);

            if (entity == null)
            {
                return ErrorCode.NotFound;
            }

            await _qtagRepository.DeleteCommitAsync(entity);

            return ErrorCode.Success;
        }

        private async Task<QTag> FindAsync(string name)
        {
            return await _qtagRepository.QueryNoTracking
                .Filter(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                .GetSingleAsync();
        }
    }
}
