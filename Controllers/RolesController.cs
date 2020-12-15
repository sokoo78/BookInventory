using BookInventory.Data;
using BookInventory.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace BookInventory.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        
        public IActionResult Index()
        {
            var roles = _db.Roles.ToList();
            
            return View(roles);
        }

        public IActionResult Upsert(string id)
        {
            if (string.IsNullOrEmpty(id)) return View(); // Insert
            else // update
            {
                var dbObject = _db.Roles.FirstOrDefault(r => r.Id == id);
                return View(dbObject);
            }            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(IdentityRole role)
        {
            if (await _roleManager.RoleExistsAsync(role.Name)) // Error
            {
                TempData[SD.Error] = "Role already exists.";
                return RedirectToAction(nameof(Index));
            }
            if (string.IsNullOrEmpty(role.Id)) // Create
            {
                await _roleManager.CreateAsync(new IdentityRole() { Name = role.Name });
                TempData[SD.Success] = "Role created successfully.";
            }
            else // Update
            {
                var dbObject = _db.Roles.FirstOrDefault(r => r.Id == role.Id);
                if (dbObject == null)
                {
                    TempData[SD.Error] = "Role does not exists.";
                    return RedirectToAction(nameof(Index));
                }
                dbObject.Name = role.Name;
                dbObject.NormalizedName = role.Name.ToUpper();
                var result = await _roleManager.UpdateAsync(dbObject);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var dbObject = _db.Roles.FirstOrDefault(r => r.Id == id);
            if (dbObject == null)
            {
                TempData[SD.Error] = "Role cannot be found.";
                return RedirectToAction(nameof(Index));
            }
            
            var userRolesForThisRole = _db.UserRoles.Where(u => u.RoleId == id).Count();
            if (userRolesForThisRole > 0)
            {
                TempData[SD.Error] = "Cannot delete role, because there are users assigned to it.";
                return RedirectToAction(nameof(Index));
            }

            await _roleManager.DeleteAsync(dbObject);
            TempData[SD.Success] = "Role has been succesfully deleted.";

            return RedirectToAction(nameof(Index));
        }
    }
}
