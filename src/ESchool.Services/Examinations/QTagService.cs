using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESchool.Data;
using ESchool.Domain.Entities.Examinations;
using ESchool.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ESchool.Services.Examinations
{
    public class QTagService : BaseService<QTag>, IQTagService
    {
        public QTagService(ObjectDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<IEnumerable<QTag>> GetListAsync()
        {
            return await DbSetNoTracking
                .OrderBy(t => t.Name)
                .ToListAsync();
        }

        public override async Task<ErrorCode> CreateAsync(QTag entity)
        {
            var duplicateEntity = await FindAsync(entity.Name);

            if (duplicateEntity != null)
            {
                return ErrorCode.DuplicateEntity;
            }

            return await base.CreateAsync(entity);
        }

        public override async Task<ErrorCode> UpdateAsync(int id, QTag entity)
        {
            var updatedEntity = await FindAsync(id);

            if (updatedEntity == null)
            {
                return ErrorCode.NotFound;
            }

            var duplicateEntity = await FindAsync(entity.Name);

            if (duplicateEntity != null && duplicateEntity.Id != id)
            {
                return ErrorCode.DuplicateEntity;
            }

            updatedEntity.Name = entity.Name;
            updatedEntity.Description = entity.Description;

            return await CommitAsync();
        }

        private async Task<QTag> FindAsync(string name)
        {
            return await DbSetNoTracking
                .SingleOrDefaultAsync(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
