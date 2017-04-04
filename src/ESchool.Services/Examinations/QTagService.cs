using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESchool.Data.Repositories;
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

        public async Task<int> CreateAsync(QTag entity)
        {
            return await _qtagRepository.CreateCommitAsync(entity);
        }

        public async Task<int> UpdateAsync(QTag entity)
        {
            return await _qtagRepository.UpdateCommitAsync(entity);
        }

        public async Task<int> DeleteAsync(QTag entity)
        {
            return await _qtagRepository.DeleteCommitAsync(entity);
        }
    }
}
