using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESchool.Data;
using ESchool.Domain.DTOs.Examinations;
using ESchool.Domain.Entities.Examinations;
using ESchool.Domain.Extensions;
using ESchool.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

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
            var qtags = await QTags.AsNoTracking()
                .Include(t => t.Group)
                .ToListAsync();

            if (qtags.Count == 0)
            {
                return Enumerable.Empty<QTagDto>();
            }

            IList<QTagDto> hierarchyQTags = new List<QTagDto>();

            var rootQTags = qtags
                .Where(t => t.ParentId == 0)
                .ToList();

            QTagDto hierarchyQTag;

            foreach (var qtag in rootQTags)
            {
                hierarchyQTag = qtag.ToQTagDto();
                hierarchyQTags.Add(hierarchyQTag);

                GetHierarchyQTags(qtags, hierarchyQTag);
            }

            return hierarchyQTags;
        }

        public async Task<QTag> CreateAsync(QTag entity)
        {
            var duplicateEntity = await FindAsync(entity.Group.Id, entity.Name);

            if (duplicateEntity != null)
            {
                throw new EntityDuplicateException("QTag Name is duplicated.");
            }

            await QTags.AddAsync(entity);
            await CommitAsync();

            return entity;
        }

        public async Task<int> UpdateAsync(QTag entity)
        {
            var updatedEntity = await FindAsync(entity.Id);

            if (updatedEntity == null)
            {
                throw new EntityNotFoundException("QTag not found.");
            }

            var duplicateEntity = await FindAsync(entity.GroupId, entity.Name);

            if (duplicateEntity != null && duplicateEntity.Id != entity.Id)
            {
                throw new EntityDuplicateException("QTag Name is duplicated.");
            }

            updatedEntity.Name = entity.Name;
            updatedEntity.Description = entity.Description;
            updatedEntity.Group = entity.Group;

            return await CommitAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var entity = await FindAsync(id);

            if (entity == null)
            {
                throw new EntityNotFoundException("QTag not found.");
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

        private async Task<QTag> FindAsync(int groupId, string name)
        {
            return await QTags.AsNoTracking()
                .SingleOrDefaultAsync(t => t.GroupId == groupId && t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        private void GetHierarchyQTags(IList<QTag> allQTags, QTagDto hierarchyQTag)
        {
            var subQTags = allQTags.Where(m => m.ParentId == hierarchyQTag.Id);

            QTagDto subHierarchyQTag;

            foreach (var qtag in subQTags)
            {
                subHierarchyQTag = qtag.ToQTagDto();
                hierarchyQTag.SubQTags.Add(subHierarchyQTag);

                GetHierarchyQTags(allQTags, subHierarchyQTag);
            }
        }
    }
}
