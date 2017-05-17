using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESchool.Domain.Entities.Accounts;
using ESchool.Domain.Extensions;
using ESchool.Domain.ViewModels.Accounts;
using ESchool.Services.Exceptions;
using ESchool.Services.Infrastructure;
using ESchool.Services.Infrastructure.Cache;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ESchool.Admin.Controllers
{
    public class AccountsController : AdminController
    {
        private readonly ILogger<AccountsController> _logger;
        private readonly IMemoryCacheService _memoryCacheService;

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountsController(
            ILogger<AccountsController> logger,
            IMemoryCacheService memoryCacheService,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _memoryCacheService = memoryCacheService;

            _roleManager = roleManager;
            _userManager = userManager;
            //_emailAccountService = emailAccountService;
            //_queuedEmailService = queuedEmailService;
        }

        [HttpGet("{id}")]
        public async Task<ApplicationUser> Get(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        //[HttpGet]
        //public async Task<IPagedList<ApplicationUser>> Get()
        //{
        //    return await _groupService.GetListAsync();
        //}

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AccountViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = viewModel.ToApplicationUser();
                var result = await _userManager.CreateAsync(user, viewModel.Password);

                if (result.Succeeded)
                {
                    if (viewModel.Roles != null && viewModel.Roles.Length > 0)
                    {
                        result = await _userManager.AddToRolesAsync(user, viewModel.Roles);

                        if (!result.Succeeded)
                        {
                            return BadRequest(result.Errors);
                        }
                    }

                    return Created("Post", user.Id);
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]AccountViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                {
                    return NotFound();
                }

                // Assign Roles.
                IdentityResult result;
                var currentRoles = await _userManager.GetRolesAsync(user);

                if (viewModel.Roles == null || viewModel.Roles.Length == 0)
                {
                    result = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                }
                else
                {
                    var roles = await GetRoles();

                    var rolesNotExists = viewModel.Roles.Except(roles.Select(r => r.Name));

                    if (rolesNotExists.Any())
                    {
                        return BadRequestErrorDto(ErrorCode.Undefined, $"Roles '{string.Join(",", rolesNotExists)}' does not exist in the system.");
                    }

                    result = await _userManager.RemoveFromRolesAsync(user, currentRoles);

                    if (result.Succeeded)
                    {
                        result = await _userManager.AddToRolesAsync(user, viewModel.Roles);

                        if (result.Succeeded)
                        {
                            return NoContent();
                        }
                    }
                }

                return BadRequest(result.Errors);
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                {
                    return NotFound();
                }

                var result = await _userManager.DeleteAsync(user);

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                return NoContent();
            }

            return BadRequestErrorDto(ErrorCode.InvalidEntityId, "Invalid Application User Id.");
        }

        [HttpPut("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);

                if (user == null)
                {
                    return NotFound();
                }

                var result = await _userManager.ChangePasswordAsync(user, viewModel.OldPassword, viewModel.NewPassword);

                if (result.Succeeded)
                {
                    return NoContent();
                }

                return BadRequest(result.Errors);
            }

            return BadRequest(ModelState);
        }

        [NonAction]
        private async Task<IEnumerable<IdentityRole>> GetRoles()
        {
            return await _memoryCacheService.GetSlidingExpiration(
                ApplicationConsts.RolesKey,
                () =>
                {
                    return _roleManager.Roles.ToListAsync();
                });
        }
    }
}
