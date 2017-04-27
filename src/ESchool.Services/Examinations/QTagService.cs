using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESchool.Data;
using ESchool.Domain.DTOs.Examinations;
using ESchool.Domain.Entities.Examinations;
using ESchool.Domain.Enums;
using ESchool.Domain.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ESchool.Services.Examinations
{
    public class QTagService : BaseService, IQTagService
    {
        public QTagService(ObjectDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<QTag> FindAsync(int id)
        {
            return await QTags.FindAsync(id);
        }

        public async Task<QTagDto> GetAsync(int id)
        {
            var entity = await QTags.FindAsync(id);

            if (entity == null)
            {
                return null;
            }

            return entity.ToQTagDto();
        }

        public async Task<IEnumerable<QTagDto>> GetListAsync()
        {
            return await QTags.AsNoTracking()
                .OrderBy(t => t.Id)
                .Select(t => t.ToQTagDto())
                .ToListAsync();
        }

        public async Task<ErrorCode> CreateAsync(QTag entity)
        {
            var duplicateEntity = await FindAsync(entity.Name);

            if (duplicateEntity != null)
            {
                return ErrorCode.DuplicateEntity;
            }

            await QTags.AddAsync(entity);

            return await CommitAsync();
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

            return await CommitAsync();
        }

        public async Task<ErrorCode> DeleteAsync(int id)
        {
            var entity = await FindAsync(id);

            if (entity == null)
            {
                return ErrorCode.NotFound;
            }

            QTags.Remove(entity);

            return await CommitAsync();
        }

        private DbSet<QTag> QTags
        {
            get
            {
                return _dbContext.Set<QTag>();
            }
        }

        private async Task<QTag> FindAsync(string name)
        {
            return await QTags.AsNoTracking()
                .SingleOrDefaultAsync(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
