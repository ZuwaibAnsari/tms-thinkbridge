using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using TMSMVC.Models;

namespace TMSMVC.Controllers
{
    public class ControllerBase<T> : Controller
    {
        protected async Task<ActionResult<T>> Get(string url, string userId = null)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_URI);
                if (!string.IsNullOrEmpty(userId))
                    client.DefaultRequestHeaders.Add("UserId", userId);
                //HTTP GET
                var responseTask = client.GetAsync(url);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var data = JsonConvert.DeserializeObject<T>(await result.Content.ReadAsStringAsync());

                    return Ok(data);
                }
                else //web api sent error response 
                {
                    //log response status here..

                    //students = Enumerable.Empty<StudentViewModel>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    return StatusCode((int)HttpStatusCode.InternalServerError, "ErrorMessage: Server error. Please contact administrator.");
                }
            }
        }
        protected async Task<ActionResult<IEnumerable<T>>> GetAll(string url, string userId = null)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_URI);
                if (!string.IsNullOrEmpty(userId))
                    client.DefaultRequestHeaders.Add("UserId", userId);
                //HTTP GET
                var responseTask = client.GetAsync(url);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var data = JsonConvert.DeserializeObject<IEnumerable<T>>(await result.Content.ReadAsStringAsync());

                    return Ok(data);
                }
                else //web api sent error response 
                {
                    //log response status here..

                    //students = Enumerable.Empty<StudentViewModel>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    return StatusCode((int)HttpStatusCode.InternalServerError, "ErrorMessage: Server error. Please contact administrator.");
                }
            }
        }
        protected static async Task<IEnumerable<Company>> GetAllCompanies()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_URI);
                //HTTP GET
                var responseTask = client.GetAsync("Companies");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var data = JsonConvert.DeserializeObject<IEnumerable<Company>>(await result.Content.ReadAsStringAsync());

                    return data;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    //students = Enumerable.Empty<StudentViewModel>();

                    //ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    return null;
                }
            }
        }

        protected static async Task<IEnumerable<Department>> GetAllDepartments()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_URI);
                //HTTP GET
                var responseTask = client.GetAsync("Departments");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var data = JsonConvert.DeserializeObject<IEnumerable<Department>>(await result.Content.ReadAsStringAsync());

                    return data;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    //students = Enumerable.Empty<StudentViewModel>();

                    //ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    return null;
                }
            }
        }

        protected static async Task<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_URI);
                //HTTP GET
                var responseTask = client.GetAsync("Users");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var data = JsonConvert.DeserializeObject<IEnumerable<ApplicationUser>>(await result.Content.ReadAsStringAsync());

                    return data;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    //students = Enumerable.Empty<StudentViewModel>();

                    //ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    return null;
                }
            }
        }

        protected async Task<ActionResult<T>> Post(string url, T model)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_URI);

                //HTTP POST
                var postTask = client.PostAsJsonAsync<T>(url, model);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var data = JsonConvert.DeserializeObject<T>(await result.Content.ReadAsStringAsync());

                    return Ok(data);
                }
            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return StatusCode((int)HttpStatusCode.InternalServerError, "ErrorMessage: Server error. Please contact administrator.");
        }

        protected async Task<ActionResult<T>> Put(string url, T model, string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_URI);

                //HTTP PUT
                var putTask = client.PutAsJsonAsync<T>(url + "/" + id, model);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var data = JsonConvert.DeserializeObject<T>(await result.Content.ReadAsStringAsync());

                    return Ok(data);
                }
            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return StatusCode((int)HttpStatusCode.InternalServerError, "ErrorMessage: Server error. Please contact administrator.");
        }
        protected async Task<ActionResult<List<TasksDueReport>>> GetTaskDueReport(string userId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_URI);
                if (string.IsNullOrEmpty(userId))
                    return NotFound();

                client.DefaultRequestHeaders.Add("UserId", userId);
                //HTTP GET
                var responseTask = client.GetAsync("TaskItems/DueReport");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var data = JsonConvert.DeserializeObject<List<TasksDueReport>>(await result.Content.ReadAsStringAsync());

                    return Ok(data);
                }
                else //web api sent error response 
                {
                    //log response status here..

                    //students = Enumerable.Empty<StudentViewModel>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    return StatusCode((int)HttpStatusCode.InternalServerError, "ErrorMessage: Server error. Please contact administrator.");
                }
            }
        }


        private static string BASE_URI = "https://localhost:44338/api/";
    }
}
