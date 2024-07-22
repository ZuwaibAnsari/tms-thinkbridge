using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TMSMVC.Models;

namespace TMSMVC.Controllers
{
    public class TaskItemsController : ControllerBase<TaskItem>
    {
        public TaskItemsController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: TaskItemsController/DueReport
        public async Task<ActionResult> TasksDueReport()
        {
            _currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var result = await GetTaskDueReport(_currentUser.Id);
            var result1 = (OkObjectResult)result.Result;

            //ViewBag.Departments = await GetAllDepartments();
            //ViewBag.Users = await GetAllUsers();

            return View(result1?.Value);
        }

        // GET: TaskItemsController
        public async Task<ActionResult> Index()
        {
            _currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var result = await GetAll("TaskItems", _currentUser.Id);
            var result1 = (OkObjectResult)result.Result;

            ViewBag.Departments = await GetAllDepartments();
            ViewBag.Users = await GetAllUsers();

            return View(result1?.Value);
        }

        // GET: TaskItemsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TaskItemsController/Create
        public async Task<ActionResult> Create()
        {
            var departments = await GetAllDepartments();
            ViewBag.Departments = departments.Select(c => new SelectListItem { Value = c.Id, Text = c.Name }).ToList();
            return View();
        }

        // POST: TaskItemsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                _currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
                TaskItem taskItem = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = collection["Title"],
                    Description = collection["Description"],
                    AssignedToDepartmentID = collection["AssignedToDepartmentID"],
                    AssignedToUserID = collection["AssignedToUserID"],
                    ExpectedCompletionDate = Convert.ToDateTime(collection["ExpectedCompletionDate"]),
                    Status = (TaskStatuses)Convert.ToInt32(collection["Status"]),
                    Priority = (TaskPriorities)Convert.ToInt32(collection["Priority"]),
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                    CreatedBy = _currentUser.Id,
                    ModifiedBy = _currentUser.Id,
                    IsActive = true,
                    OwnerID = _currentUser.CompanyId ?? ""
                };
                var result = await Post("TaskItems", taskItem);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TaskItemsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TaskItemsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: TaskItemsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TaskItemsController/Delete/5
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
