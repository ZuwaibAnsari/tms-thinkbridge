using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TMSMVC.Models;

namespace TMSMVC.Controllers
{
    public class CompaniesController : ControllerBase<Company>
    {
        public CompaniesController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        // GET: CompaniesController
        public async Task<ActionResult> Index()
        {
            var result = await GetAll("Companies");
            var result1 = (OkObjectResult)result.Result;
            ViewBag.Users = await GetAllUsers();
            return View(result1?.Value);
        }

        // GET: CompaniesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CompaniesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CompaniesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                _currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
                Company company = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = collection["Name"],
                    AdminID = "",
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                    CreatedBy = _currentUser.Id,
                    ModifiedBy = _currentUser.Id,
                    IsActive = true,
                    OwnerID = _currentUser.CompanyId ?? ""
                };
                var result = await Post("Companies", company);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CompaniesController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var result = await Get("Companies/" + id);
            var result1 = (OkObjectResult)result.Result;
            var users = await GetAllUsers();
            ViewBag.Users = users.Select(c => new SelectListItem { Value = c.Id, Text = c.FullName }).ToList();
            return View(result1?.Value);
        }

        // POST: CompaniesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, IFormCollection collection)
        {
            try
            {
                _currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
                var result = await Get("Companies/" + id);
                var result1 = (OkObjectResult)result.Result;
                Company company = (Company)result1?.Value;
                if (company != null)
                {
                    company.Name = collection["Name"];
                    company.AdminID = collection["AdminID"];
                    company.Modified = DateTime.Now;
                    company.ModifiedBy = _currentUser.Id;
                    company.OwnerID = _currentUser.CompanyId ?? "";
                }
                var putResult = await Put("Companies", company, company.Id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CompaniesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CompaniesController/Delete/5
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
