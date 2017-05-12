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

        public async Task<QTagDto> GetAsync(int id, bool includeParents = false)
        {
            if (!includeParents)
            {
                return await GetWithoutParentsAsync(id);
            }

            //var entity = await QTags.FindAsync(id);

            //if (entity == null)
            //{
            //    return null;
            //}

            //var qtagDto = entity.ToQTagDto();

            //if (!includeParents)
            //{
            //    qtagDto.SubQTags = await QTags.AsNoTracking()
            //        .Where(t => t.ParentId == entity.Id)
            //        .Select(t => new QTagDto
            //        {
            //            ParentId = t.ParentId,
            //            Id = t.Id,
            //            Name = t.Name,
            //            Description = t.Description
            //        }).ToListAsync();
            //}

            //var qtags = await QTags.AsNoTracking()
            //    .Where(t => t.GroupId == entity.GroupId)
            //    .ToListAsync();

            //if (qtagDto.ParentId != 0)
            //{
            //    qtagDto.ParentQTags = GetParentQTags(qtags, qtagDto.ParentId);
            //}

            //qtagDto.SubQTags = qtags.Where(t => t.ParentId == entity.Id)
            //    .Select(t => new QTagDto
            //    {
            //        ParentId = t.ParentId,
            //        Id = t.Id,
            //        Name = t.Name,
            //        Description = t.Description
            //    }).ToList();

            return null;
        }

        public async Task<IList<QTagDto>> GetListAsync(int groupId)
        {
            return await QTags.AsNoTracking()
                .Where(t => t.GroupId == groupId && t.ParentId == 0)
                .Select(t => t.ToQTagDto())
                .ToListAsync();
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

        private async Task<QTagDto> GetWithoutParentsAsync(int id)
        {
            var qtags = await QTags.AsNoTracking()
                .Where(t => t.Id == id || t.ParentId == id)
                .ToListAsync();

            QTagDto qtagDto = null;
            IList<QTagDto> subQTagDtos = new List<QTagDto>();

            foreach (var qtag in qtags)
            {
                if (qtag.Id == id)
                {
                    qtagDto = qtag.ToQTagDto();
                }
                else
                {
                    subQTagDtos.Add(qtag.ToQTagDto());
                }
            }

            qtagDto.SubQTags = subQTagDtos;

            return qtagDto;
        }

        private IList<IdNameDto> GetParentQTags(IList<QTag> allQTags, int parentId)
        {
            IList<IdNameDto> parentQTags = new List<IdNameDto>();
            QTag qtag;

            while (parentId != 0)
            {
                qtag = allQTags.SingleOrDefault(t => t.Id == parentId);

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

