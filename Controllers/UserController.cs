using BookInventory.Authorization;
using BookInventory.Data;
using BookInventory.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookInventory.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        
        public IActionResult Index()
        {
            var userList = _db.ApplicationUser.ToList();
            var userRoleList = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();
            foreach (var user in userList)
            {
                var role = userRoleList.FirstOrDefault(u => u.UserId == user.Id);
                if (role == null || roles.Count() < 1) user.Role = "None";
                else user.Role = roles.FirstOrDefault(r => r.Id == role.RoleId).Name;
            }
            
            return View(userList);
        }

        public IActionResult Edit(string userId)
        {
            var dbUserObject = _db.ApplicationUser.FirstOrDefault(u => u.Id == userId);
            if (dbUserObject == null) return NotFound();
            
            var userRole = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();
            var role = userRole.FirstOrDefault(r => r.UserId == dbUserObject.Id);
            if (role != null)
            {
                dbUserObject.RoleId = roles.FirstOrDefault(r => r.Id == role.RoleId).Id;
            }

            dbUserObject.RoleList = _db.Roles.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id
            });

            return View(dbUserObject);
        }

        public async Task<IActionResult> ManageUserClaims(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var currentClaims = await _userManager.GetClaimsAsync(user);
            var model = new UserClaimsViewModel() { UserId = userId };
            foreach (var claim in ClaimStore.Claims)
            {
                var userClaim = new UserClaim { ClaimType = claim.Type };
                if (currentClaims.Any(c => c.Type == claim.Type))
                {   // Claim exists and is selected
                    userClaim.IsSelected = true;
                }
                model.Claims.Add(userClaim);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationUser user)
        {
            var dbUserObject = _db.ApplicationUser.FirstOrDefault(u => u.Id == user.Id);
            if (dbUserObject == null) return NotFound();
            
            var userRole = _db.UserRoles.FirstOrDefault(r => r.UserId == dbUserObject.Id);
            if (userRole != null) // Old role must be removed
            {
                var currentRoleName = _db.Roles.Where(r => r.Id == userRole.RoleId).Select(r => r.Name).FirstOrDefault();
                await _userManager.RemoveFromRoleAsync(dbUserObject, currentRoleName);
            }
            
            // Add the new role
            await _userManager.AddToRoleAsync(dbUserObject, _db.Roles.FirstOrDefault(r => r.Id == user.RoleId).Name);
            dbUserObject.UserName = user.UserName;
            await _db.SaveChangesAsync();
            TempData[SD.Success] = "User has been updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel userClaimsViewModel)
        {
            var user = await _userManager.FindByIdAsync(userClaimsViewModel.UserId);
            if (user == null) return NotFound();
            
            var selectedClaims = userClaimsViewModel.Claims.Where(c => c.IsSelected)
                .Select(c => new Claim(c.ClaimType, c.IsSelected.ToString()));

            var currentClaims = await _userManager.GetClaimsAsync(user);
            var result = await _userManager.RemoveClaimsAsync(user, currentClaims);
            if (!result.Succeeded)
            {
                TempData[SD.Error] = "Failed to update claims.";
                return View(userClaimsViewModel);
            }
            
            result = await _userManager.AddClaimsAsync(user, selectedClaims);
            if (!result.Succeeded)
            {
                TempData[SD.Error] = "Failed to update claims.";
                return View(userClaimsViewModel);
            }

            TempData[SD.Success] = "Claims has been updated successfully";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> LockUnlock(string userId)
        {
            var dbUserObject = _db.ApplicationUser.FirstOrDefault(u => u.Id == userId);
            if (dbUserObject == null) return NotFound();            

            if (dbUserObject.LockoutEnd != null && dbUserObject.LockoutEnd > DateTime.Now)
            {   // User is currently locked, must be unlocked

                dbUserObject.LockoutEnd = DateTime.Now;
                TempData[SD.Success] = "User has been unlocked successfully.";
            }
            else
            {   // User us currently not locked, must be locked
                dbUserObject.LockoutEnd = DateTime.Now.AddYears(1000);
                TempData[SD.Success] = "User has been locked successfully.";
            }
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string userId)
        {
            var dbUserObject = _db.ApplicationUser.FirstOrDefault(u => u.Id == userId);
            if (dbUserObject == null) return NotFound();
            
            _db.ApplicationUser.Remove(dbUserObject);
            await _db.SaveChangesAsync();
            TempData[SD.Success] = "User has been deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
