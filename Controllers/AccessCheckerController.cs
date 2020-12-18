using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookInventory.Controllers
{
    [Authorize]
    public class AccessCheckerController : Controller
    {
        [AllowAnonymous]
        public IActionResult All()
        {
            return View();
        }

        public IActionResult AuthorizedOnly()
        {
            return View();
        }

        [Authorize(Roles = "User")]
        public IActionResult WithUserRoleOnly()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult WithAdminRoleOnly()
        {
            return View();
        }

        [Authorize(Roles = "User,Admin")]
        public IActionResult WithUserORAdminRoleOnly()
        {
            return View();
        }

        [Authorize(Policy = "UserAndAdmin")]
        public IActionResult WithUserANDAdminRoleOnly()
        {
            return View();
        }

        [Authorize(Policy = "Adult")]
        public IActionResult AuthorizedWithAdultClaimOnly()
        {
            return View();
        }
    }
}
