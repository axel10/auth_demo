using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers
{
    public class AuthTestController : Controller
    {
        
        public IActionResult AddNews()
        {
            return Content("addnews");
        }

        public IActionResult RemoveNews()
        {
            return Content("RemoveNews");
        }

        [Authorize()]
        public IActionResult EditNews()
        {
            return Content("EditNews");
        }


        [Authorize(Policy = "ManagerOnly")]
        public IActionResult AddEmp()
        {
            return Content("AddEmp");
        }
        [Authorize(Policy = "Emp")]
        public IActionResult Work()
        {
            return Content("Work");
        }
    }
}