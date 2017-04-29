using System.Threading.Tasks;
using ESchool.Domain.Entities.Systems;
using ESchool.Domain.ViewModels.Systems;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ESchool.Admin.Controllers
{
    public class AccountController : AdminController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly IClientStore _clientStore;
        private readonly IPersistedGrantService _persistedGrantService;
        private readonly IIdentityServerInteractionService _interaction;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IClientStore clientStore,
            IIdentityServerInteractionService interaction,
            IPersistedGrantService persistedGrantService)
        {
            _userManager = userManager;
            _signInManager = signInManager;

            _clientStore = clientStore;
            _persistedGrantService = persistedGrantService;
            _interaction = interaction;
        }

        //[AllowAnonymous]
        //[HttpPost(Name = "Login")]
        //public async Task<IActionResult> Login([FromBody]LoginViewModel viewModel)
        //{
        //    if (ModelState.IsValid)
        //    {

        //    }

        //    return BadRequest(viewModel);
        //}
    }
}
