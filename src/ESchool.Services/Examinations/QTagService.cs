using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESchool.Data.Repositories;
using ESchool.Domain;
using ESchool.Domain.Entities.Examinations;

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

        public async Task<QTag> FindAsync(string name)
        {
            return await _qtagRepository.GetSingleAsync(
                t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<IEnumerable<QTag>> GetListAsync()
        {
            return await _qtagRepository.Query
                .Sort(o => o.OrderBy(t => t.Name))
                .GetListAsync();
        }

        public async Task<ErrorCode> CreateAsync(QTag entity)
        {
            var qtag = await FindAsync(entity.Name);

            if (qtag != null)
            {
                return ErrorCode.DuplicateEntity;
            }

            await _qtagRepository.CreateCommitAsync(entity);

            return ErrorCode.Success;
        }

        public async Task<ErrorCode> UpdateAsync(QTag entity)
        {
            var qtag = await FindAsync(entity.Name);

            if (qtag != null && qtag.Id != entity.Id)
            {
                return ErrorCode.DuplicateEntity;
            }

            await _qtagRepository.UpdateCommitAsync(entity);

            return ErrorCode.Success;
        }

        public async Task<ErrorCode> DeleteAsync(int id)
        {
            var entity = await FindAsync(id);

            if (entity != null)
            {
                await _qtagRepository.DeleteCommitAsync(entity);
            }

            return ErrorCode.Success;
        }
    }
}
