using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESchool.Data;
using ESchool.Domain.DTOs;
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

        public async Task<QTagDto> GetAsync(int id)
        {
            var entity = await QTags.FindAsync(id);

            if (entity == null)
            {
                return null;
            }

            var qtags = await QTags.AsNoTracking()
                .Where(t => t.GroupId == entity.GroupId)
                .Select(t => t.ToQTagDto())
                .ToListAsync();

            var qtagDto = entity.ToQTagDto();

            if (qtagDto.ParentId != 0)
            {
                qtagDto.ParentQTags = GetParentQTags(qtags, qtagDto.ParentId);
            }

            var children = qtags.Where(t => t.ParentId == qtagDto.Id).ToList();

            if (children.Any())
            {
                foreach (var qtag in children)
                {
                    qtag.SubQTagsCount = qtags.Count(t => t.ParentId == qtag.Id);
                }

                qtagDto.SubQTags = children;
            }

            return qtagDto;
        }

        public async Task<IList<QTagDto>> GetListAsync(int groupId)
        {
            IList<QTagDto> qtagDtos = new List<QTagDto>();

            var qtags = await QTags.AsNoTracking()
                .Where(t => t.GroupId == groupId)
                .Select(t => t.ToQTagDto())
                .ToListAsync();

            foreach (var qtag in qtags)
            {
                if (qtag.ParentId == 0)
                {
                    qtag.SubQTagsCount = qtags.Count(t => t.ParentId == qtag.Id);
                    qtagDtos.Add(qtag);
                }
            }

            return qtagDtos;
        }

        public async Task<QTag> CreateAsync(QTag entity)
        {
            var duplicateEntity = await FindAsync(entity.Name, entity.GroupId, entity.ParentId);

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
            var updatedEntity = await QTags
                .Include(t => t.Group)
                .SingleOrDefaultAsync(t => t.Id == entity.Id);

            if (updatedEntity == null)
            {
                throw new EntityNotFoundException("QTag not found.");
            }

            var duplicateEntity = await FindAsync(entity.Name, entity.GroupId, entity.ParentId);

            if (duplicateEntity != null && duplicateEntity.Id != entity.Id)
            {
                throw new EntityDuplicateException("QTag Name is duplicated.");
            }

            updatedEntity.ParentId = entity.ParentId;
            updatedEntity.GroupId = entity.GroupId;
            updatedEntity.Name = entity.Name;
            updatedEntity.Description = entity.Description;

            return await CommitAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var entity = await QTags.FindAsync(id);

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

        private async Task<QTag> FindAsync(string name, int groupId, int parentId)
        {
            return await QTags.AsNoTracking()
                .SingleOrDefaultAsync(t =>
                    t.GroupId == groupId &&
                    t.ParentId == parentId &&
                    t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        private IList<IdNameDto> GetParentQTags(IList<QTagDto> qtags, int parentId)
        {
            QTagDto qtag;
            IList<IdNameDto> parentQTags = new List<IdNameDto>();

            while (parentId != 0)
            {
                qtag = qtags.SingleOrDefault(t => t.Id == parentId);

                if (qtag == null)
                {
                    break;
                }

                parentId = qtag.ParentId;
                parentQTags.Insert(0, new IdNameDto(qtag.Id, qtag.Name));
            }

            return parentQTags;
        }
    }
}

