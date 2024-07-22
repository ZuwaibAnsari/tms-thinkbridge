using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMSMVC.Data;
using TMSMVC.Models;

namespace TMSMVC.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TaskItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/TaskItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTaskItem()
        {
            List<TaskItem> result = new();

            var userId = HttpContext.Request.Headers["UserId"];

            var userController = new UsersController(_context);
            var userActionResult = await userController.GetUser(userId);
            ApplicationUser currentUser = userActionResult.Value;

            //Return all Tasks for SuperAdmin
            if (currentUser.IsSuperAdmin)
                return await _context.TaskItem.ToListAsync();

            var companyController = new CompaniesController(_context);
            var userCompanyActionResult = await companyController.GetCompany(currentUser.CompanyId);
            Company currentUserCompany = userCompanyActionResult.Value;

            var deptController = new DepartmentsController(_context);
            var userDeptActionResult = await deptController.GetDepartment(currentUser.DepartmentId);
            Department currentUserDept = userDeptActionResult.Value;

            if (currentUser != null && currentUserCompany != null && currentUserDept != null)
            {
                //If company admin, return all Tasks
                if (currentUserCompany.AdminID == currentUser.Id)
                    result = await _context.TaskItem.ToListAsync();

                //Else if department admin, return all department Tasks
                else if (currentUserDept.AdminID == currentUser.Id)
                    result = await _context.TaskItem.Where(t => t.AssignedToDepartmentID == currentUser.DepartmentId).ToListAsync();

                //Else return only assigned Tasks
                else
                    result = await _context.TaskItem.Where(t => t.AssignedToUserID == currentUser.Id).ToListAsync();
            }

            return result;
        }

        // GET: api/TaskItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetTaskItem(string id)
        {
            var taskItem = await _context.TaskItem.FindAsync(id);

            if (taskItem == null)
            {
                return NotFound();
            }

            return taskItem;
        }

        // PUT: api/TaskItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskItem(string id, TaskItem taskItem)
        {
            if (id != taskItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(taskItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TaskItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TaskItem>> PostTaskItem(TaskItem taskItem)
        {
            _context.TaskItem.Add(taskItem);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (TaskItemExists(taskItem.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTaskItem", new { id = taskItem.Id }, taskItem);
        }

        // DELETE: api/TaskItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskItem(string id)
        {
            var taskItem = await _context.TaskItem.FindAsync(id);
            if (taskItem == null)
            {
                return NotFound();
            }

            _context.TaskItem.Remove(taskItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/TaskItems/DueReport
        [HttpGet]
        [Route("DueReport")]
        public async Task<ActionResult<List<TasksDueReport>>> GetTaskDueReport()
        {
            List<TasksDueReport> report = new();
            List<TaskItem> taskList = new();
            bool isCompanyAdmin = false;
            bool isDeptAdmin = false;

            var userId = HttpContext.Request.Headers["UserId"];

            var userController = new UsersController(_context);
            var userActionResult = await userController.GetUser(userId);
            ApplicationUser currentUser = userActionResult.Value;

            //Return all Tasks for SuperAdmin
            if (currentUser.IsSuperAdmin)
                taskList = await _context.TaskItem.ToListAsync();

            var companyController = new CompaniesController(_context);
            var userCompanyActionResult = await companyController.GetCompany(currentUser.CompanyId);
            Company currentUserCompany = userCompanyActionResult.Value;

            var deptController = new DepartmentsController(_context);
            var userDeptActionResult = await deptController.GetDepartment(currentUser.DepartmentId);
            Department currentUserDept = userDeptActionResult.Value;

            if (currentUser != null && currentUserCompany != null && currentUserDept != null)
            {
                isCompanyAdmin = (currentUserCompany.AdminID == currentUser.Id);
                isDeptAdmin = (currentUserDept.AdminID == currentUser.Id);
                //If company admin, return all Tasks
                if (isCompanyAdmin)
                {
                    taskList = await _context.TaskItem.ToListAsync();
                    //Build report
                    var allDeptActionResult = await deptController.GetDepartment();
                    foreach (var dept in allDeptActionResult.Value)
                    {
                        string deptId = dept.Id;
                        int dueInWeekCount =
                            taskList.Where(t => t.AssignedToDepartmentID == deptId
                            && t.Status < TaskStatuses.Done
                            && Convert.ToDateTime(t.ExpectedCompletionDate) <= DateTime.Now.AddDays(7)).Count();

                        int dueInMonthCount =
                            taskList.Where(t => t.AssignedToDepartmentID == deptId
                            && t.Status < TaskStatuses.Done
                            && Convert.ToDateTime(t.ExpectedCompletionDate) <= DateTime.Now.AddDays(30)).Count();

                        report.Add(new()
                        {
                            DepartmentId = deptId,
                            DepartmentName = dept.Name,
                            DueInWeek = dueInWeekCount,
                            DueInMonth = dueInMonthCount,
                        });
                    }
                }

                //Else if department admin, return all department Tasks
                else if (isDeptAdmin)
                {
                    taskList = await _context.TaskItem.Where(t => t.AssignedToDepartmentID == currentUser.DepartmentId).ToListAsync();
                    string deptId = currentUserDept.Id;
                    int dueInWeekCount =
                        taskList.Where(t => t.AssignedToDepartmentID == deptId
                        && t.Status < TaskStatuses.Done
                        && Convert.ToDateTime(t.ExpectedCompletionDate) <= DateTime.Now.AddDays(7)).Count();

                    int dueInMonthCount =
                        taskList.Where(t => t.AssignedToDepartmentID == deptId
                        && t.Status < TaskStatuses.Done
                        && Convert.ToDateTime(t.ExpectedCompletionDate) <= DateTime.Now.AddDays(30)).Count();

                    report.Add(new()
                    {
                        DepartmentId = deptId,
                        DepartmentName = currentUserDept.Name,
                        DueInWeek = dueInWeekCount,
                        DueInMonth = dueInMonthCount,
                    });
                }

                //Else return only assigned Tasks
                else
                    taskList = await _context.TaskItem.Where(t => t.AssignedToUserID == currentUser.Id).ToListAsync();
            }

            return report;
        }

        private bool TaskItemExists(string id)
        {
            return _context.TaskItem.Any(e => e.Id == id);
        }
    }
}
