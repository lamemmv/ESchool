﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ESchool.Domain.Entities.Examinations;
using ESchool.Domain.Enums;
using ESchool.Domain.Extensions;
using ESchool.Domain.ViewModels.Examinations;
using ESchool.Services.Examinations;
using Microsoft.AspNetCore.Mvc;

namespace ESchool.Admin.Controllers
{
    public class QTagsController : AdminController
    {
        private readonly IQTagService _qtagService;

        public QTagsController(IQTagService qtagService)
        {
            _qtagService = qtagService;
        }

        [HttpGet]
        public async Task<IEnumerable<QTag>> Get()
        {
            return await _qtagService.GetListAsync();
        }

        [HttpGet("{id}")]
        public async Task<QTag> Get(int id)
        {
            return await _qtagService.FindAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]QTagViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var entity = viewModel.ToQTag();
                var code = await _qtagService.CreateAsync(entity);

                return PostResult(code, entity.Id);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]QTagViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var entity = viewModel.ToQTag();
                var code = await _qtagService.UpdateAsync(id, entity);

                return PutResult(code);
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id > 0)
            {
                var code = await _qtagService.DeleteAsync(id);

                return DeleteResult(code);
            }

            return BadRequest(ErrorCode.InvalidEntityId);
        }
    }
}
