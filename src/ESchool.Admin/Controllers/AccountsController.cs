//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using ESchool.Admin.ViewModels;
//using ESchool.Admin.ViewModels.Accounts;
//using ESchool.Data.Entities.Accounts;
//using ESchool.Services.Constants;
//using ESchool.Services.Infrastructure.Cache;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;

//namespace ESchool.Admin.Controllers
//{
//    public class AccountsController : AdminController
//    {
//        private readonly ILogger<AccountsController> _logger;
//        private readonly IMemoryCacheService _memoryCacheService;

//        private readonly RoleManager<IdentityRole> _roleManager;
//        private readonly UserManager<ApplicationUser> _userManager;

//        public AccountsController(
//            ILogger<AccountsController> logger,
//            IMemoryCacheService memoryCacheService,
//            RoleManager<IdentityRole> roleManager,
//            UserManager<ApplicationUser> userManager)
//        {
//            _logger = logger;
//            _memoryCacheService = memoryCacheService;

//            _roleManager = roleManager;
//            _userManager = userManager;
//            //_emailAccountService = emailAccountService;
//            //_queuedEmailService = queuedEmailService;
//        }

//        [HttpGet("{id}")]
//        public async Task<ApplicationUser> Get(string id)
//        {
//            return await _userManager.FindByIdAsync(id);
//        }

//        //[HttpGet]
//        //public async Task<IPagedList<ApplicationUser>> Get()
//        //{
//        //    return await _groupService.GetListAsync();
//        //}

//        [HttpPost]
//        public async Task<IActionResult> Post([FromBody]CreateAccountViewModel viewModel)
//        {
//            ApplicationUser user = viewModel.ToApplicationUser();
//            IdentityResult result = await _userManager.CreateAsync(user, viewModel.Password);

//            if (result.Succeeded)
//            {
//                if (viewModel.Roles != null && viewModel.Roles.Length > 0)
//                {
//                    result = await _userManager.AddToRolesAsync(user, viewModel.Roles);

//                    if (!result.Succeeded)
//                    {
//                        return BadRequest(result.Errors);
//                    }
//                }

//                return Created("Post", user.Id);
//            }
//            else
//            {
//                return BadRequest(result.Errors);
//            }
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> Put(string id, [FromBody]CreateAccountViewModel viewModel)
//        {
//            ApplicationUser user = await _userManager.FindByIdAsync(id);

//            if (user == null)
//            {
//                return NotFound();
//            }

//            // Assign Roles.
//            IdentityResult result;
//            IList<string> currentRoles = await _userManager.GetRolesAsync(user);

//            if (viewModel.Roles == null || viewModel.Roles.Length == 0)
//            {
//                result = await _userManager.RemoveFromRolesAsync(user, currentRoles);
//            }
//            else
//            {
//                var roles = await GetRoles();

//                var rolesNotExists = viewModel.Roles.Except(roles.Select(r => r.Name));

//                if (rolesNotExists.Any())
//                {
//                    return BadRequestApiError("Roles", $"Roles '{string.Join(",", rolesNotExists)}' does not exist in the system.");
//                }

//                result = await _userManager.RemoveFromRolesAsync(user, currentRoles);

//                if (result.Succeeded)
//                {
//                    result = await _userManager.AddToRolesAsync(user, viewModel.Roles);

//                    if (result.Succeeded)
//                    {
//                        return NoContent();
//                    }
//                }
//            }

//            return BadRequest(result.Errors);
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(string id)
//        {
//            if (string.IsNullOrEmpty(id))
//            {
//                ApplicationUser user = await _userManager.FindByIdAsync(id);

//                if (user == null)
//                {
//                    return NotFound();
//                }

//                IdentityResult result = await _userManager.DeleteAsync(user);

//                if (!result.Succeeded)
//                {
//                    return BadRequest(result.Errors);
//                }

//                return NoContent();
//            }

//            return BadRequestApiError("ApplicationUserId", "'Application User Id' should not be empty.");
//        }

//        [HttpPut("changepassword")]
//        public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordViewModel viewModel)
//        {
//            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);

//            if (user == null)
//            {
//                return NotFound();
//            }

//            IdentityResult result = await _userManager.ChangePasswordAsync(user, viewModel.OldPassword, viewModel.NewPassword);

//            if (result.Succeeded)
//            {
//                return NoContent();
//            }

//            return BadRequest(result.Errors);
//        }

//        [NonAction]
//        private async Task<IEnumerable<IdentityRole>> GetRoles()
//        {
//            return await _memoryCacheService.GetSlidingExpiration(
//                MemoryCacheKeys.RolesKey,
//                () =>
//                {
//                    return _roleManager.Roles.ToListAsync();
//                });
//        }
//    }
//}
