using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TMSMVC.Models;

namespace TMSMVC.Controllers
{
    public class DepartmentsController : ControllerBase<Department>
    {
        public DepartmentsController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: DepartmentsController
        public async Task<ActionResult> Index()
        {
            var companies = await GetAllCompanies();
            var users = await GetAllUsers();
            ViewBag.Companies = companies;
            ViewBag.Users = users;

            var result = await GetAll("Departments");
            var result1 = (OkObjectResult)result.Result;
            return View(result1?.Value);
        }

        // GET: DepartmentsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DepartmentsController/Create
        public async Task<ActionResult> Create()
        {
            var companies = await GetAllCompanies();
            ViewBag.Companies = companies.Select(c => new SelectListItem { Value = c.Id, Text = c.Name }).ToList();
            return View();
        }

        // POST: DepartmentsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                _currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
                Department department = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = collection["Name"],
                    CompanyID = collection["CompanyID"],
                    AdminID = "",
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                    CreatedBy = _currentUser.Id,
                    ModifiedBy = _currentUser.Id,
                    IsActive = true,
                    OwnerID = _currentUser.CompanyId ?? ""
                };
                var result = await Post("Departments", department);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DepartmentsController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var result = await Get("Departments/" + id);
            var result1 = (OkObjectResult)result.Result;
            var users = await GetAllUsers();
            var companies = await GetAllCompanies();
            ViewBag.Users = users.Select(c => new SelectListItem { Value = c.Id, Text = c.FullName }).ToList();
            ViewBag.Companies = companies.Select(c => new SelectListItem { Value = c.Id, Text = c.Name }).ToList();
            return View(result1?.Value);
        }

        // POST: DepartmentsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, IFormCollection collection)
        {
            try
            {
                _currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
                var result = await Get("Departments/" + id);
                var result1 = (OkObjectResult)result.Result;
                Department department = (Department)result1?.Value;
                if (department != null)
                {
                    department.Name = collection["Name"];
                    department.CompanyID = collection["CompanyID"];
                    department.AdminID = collection["AdminID"];
                    department.Modified = DateTime.Now;
                    department.ModifiedBy = _currentUser.Id;
                    department.OwnerID = _currentUser.CompanyId ?? "";
                }
                var putResult = await Put("Departments", department, department.Id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DepartmentsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DepartmentsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        private readonly UserManager<ApplicationUser> _userManager;

        private ApplicationUser _currentUser;
    }
}
