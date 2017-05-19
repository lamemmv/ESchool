using System.Collections.Generic;
using System.Threading.Tasks;
using ESchool.Admin.ViewModels;
using ESchool.Admin.ViewModels.Examinations;
using ESchool.Data.DTOs.Examinations;
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
            var entity = viewModel.ToQTag();
            await _qtagService.CreateAsync(entity);

            return Created("Post", entity.Id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]QTagViewModel viewModel)
        {
            var entity = viewModel.ToQTag(id);
            await _qtagService.UpdateAsync(entity);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id > 0)
            {
                await _qtagService.DeleteAsync(id);

                return NoContent();
            }

            return BadRequestApiError("QTagId", "'QTag Id' should not be empty.");
        }
    }
}
