using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookInventory.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class TokenController : Controller
    {
        public IActionResult Index(string token)
        {
            return View();
        }
    }
}
