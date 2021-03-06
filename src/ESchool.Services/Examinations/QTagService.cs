﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESchool.Data;
using ESchool.Data.DTOs.Examinations;
using ESchool.Data.Entities.Examinations;
using ESchool.Services.Enums;
using ESchool.Services.Models;
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
            QTag entity = await QTags.FindAsync(id);

            if (entity == null)
            {
                return null;
            }

            var qtags = await QTags.AsNoTracking()
                .Where(t => t.GroupId == entity.GroupId)
                .Select(t => ToQTagDto(t))
                .ToListAsync();

            QTagDto qtagDto = ToQTagDto(entity);

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
                .Select(t => ToQTagDto(t))
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
            QTag duplicateEntity = await FindAsync(entity.Name, entity.GroupId, entity.ParentId);

            if (duplicateEntity != null)
            {
                throw new ApiException(
                    $"'QTag Name' is duplicated. Parameters: {LogQTag(entity)}",
                    ApiErrorCode.DuplicateEntity);
            }

            await QTags.AddAsync(entity);
            await CommitAsync();

            return entity;
        }

        public async Task<int> UpdateAsync(QTag entity)
        {
            QTag updatedEntity = await QTags
                .Include(t => t.Group)
                .FirstOrDefaultAsync(t => t.Id == entity.Id);

            if (updatedEntity == null)
            {
                throw new ApiException(
                    $"{nameof(QTag)} not found. Id = {entity.Id}",
                    ApiErrorCode.NotFound);
            }

            QTag duplicateEntity = await FindAsync(entity.Name, entity.GroupId, entity.ParentId);

            if (duplicateEntity != null && duplicateEntity.Id != entity.Id)
            {
                throw new ApiException(
                    $"'QTag Name' is duplicated. Parameters: {LogQTag(entity)}",
                    ApiErrorCode.DuplicateEntity);
            }

            updatedEntity.ParentId = entity.ParentId;
            updatedEntity.GroupId = entity.GroupId;
            updatedEntity.Name = entity.Name;
            updatedEntity.Description = entity.Description;

            return await CommitAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            QTag entity = await QTags.FindAsync(id);

            if (entity == null)
            {
                throw new ApiException(
                    $"{nameof(QTag)} not found. Id = {id}",
                    ApiErrorCode.NotFound);
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
                .FirstOrDefaultAsync(t =>
                    t.GroupId == groupId &&
                    t.ParentId == parentId &&
                    t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        private IList<QTagDto> GetParentQTags(IList<QTagDto> qtags, int parentId)
        {
            QTagDto qtag;
            IList<QTagDto> parentQTags = new List<QTagDto>();

            while (parentId != 0)
            {
                qtag = qtags.FirstOrDefault(t => t.Id == parentId);

                if (qtag == null)
                {
                    break;
                }

                parentId = qtag.ParentId;
                parentQTags.Insert(0, new QTagDto { ParentId = qtag.ParentId, Id = qtag.Id, Name = qtag.Name });
            }

            return parentQTags;
        }

        private QTagDto ToQTagDto(QTag entity)
        {
            return new QTagDto
            {
                ParentId = entity.ParentId,
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description
            };
        }

        private string LogQTag(QTag entity)
        {
            return $"[GroupId] = {entity.GroupId}, [ParentId] = {entity.ParentId}, [QTagName] = {entity.Name}";
        }
    }
}

