using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Auth.Data;
using Auth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Auth.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        private readonly RoleManager<MyRole> _roleManager;

        public AuthController(RoleManager<MyRole> roleManager, ApplicationDbContext dbContext)
        {
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AddClaimsToRole()
        {
            var claimses = _dbContext.Claims.ToList();
            var myRoles = _roleManager.Roles.ToList();
            ViewBag.Roles = myRoles;
            ViewBag.Claimses = claimses;

            return View();
        }

        public async Task<IActionResult> GetRoleClaims(string roleName)
        {
            var claims = await _roleManager.GetClaimsAsync(_roleManager.Roles.SingleOrDefault(o => o.Name == roleName));
            return Json(claims);
        }

        public async Task<IActionResult> SetRoleClaims(string roleName, string claimsStr)
        {
            var claimses = JsonConvert.DeserializeObject<List<Claims>>(claimsStr);
            var role = _roleManager.Roles.SingleOrDefault(o => o.Name == roleName);
            var dbClaimses = await _roleManager.GetClaimsAsync(role);

            foreach (var claims in claimses)
            {
                if (!( dbClaimses.Any(o => o.Type == claims.Key && o.Value == claims.Value)))
                {
                    await _roleManager.AddClaimAsync(role, new Claim(claims.Key, claims.Value));
                }
            }

            var enumerable = dbClaimses.Where(o => !claimses.Any(i => i.Key == o.Type && i.Value == o.Value));

            foreach (var claim in enumerable)
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }

            return Content("true");
        }
    }
}