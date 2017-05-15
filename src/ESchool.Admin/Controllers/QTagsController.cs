using System.Collections.Generic;
using System.Threading.Tasks;
using ESchool.Domain.DTOs.Examinations;
using ESchool.Domain.Extensions;
using ESchool.Domain.ViewModels.Examinations;
using ESchool.Services.Examinations;
using ESchool.Services.Exceptions;
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

        [HttpGet("{id}")]
        public async Task<QTagDto> Get(int id)
        {
            return await _qtagService.GetAsync(id);
        }

        [HttpGet("GetByGroup/{groupId}")]
        public async Task<IList<QTagDto>> GetByGroup(int groupId)
        {
            return await _qtagService.GetListAsync(groupId);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]QTagViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var entity = viewModel.ToQTag();
                await _qtagService.CreateAsync(entity);

                return Created("Post", entity.Id);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]QTagViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var entity = viewModel.ToQTag(id);
                await _qtagService.UpdateAsync(entity);

                return NoContent();
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id > 0)
            {
                await _qtagService.DeleteAsync(id);

                return NoContent();
            }

            return BadRequestErrorDto(ErrorCode.InvalidEntityId, "Invalid QTag Id.");
        }
    }
}
