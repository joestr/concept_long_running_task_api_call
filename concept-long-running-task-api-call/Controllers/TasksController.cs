using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace concept_long_running_task_api_call.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private TaskPbdLayer taskPbdLayer = null;

        public TasksController(TaskPbdLayer taskPbdLayer)
        {
            this.taskPbdLayer = taskPbdLayer;
        }

        [HttpGet]
        [Route("{id}/Status", Name = nameof(RouteConstants.GetTasksById))]
        public ActionResult GetTask([FromRoute] Guid id) {
            Task t = null;

            try
            {
                t = taskPbdLayer.GetTaskById(id);
            }
            catch (InvalidOperationException ex)
            {
                return new NotFoundResult();
            }

            System.Threading.Tasks.Task tTask = taskPbdLayer.GetTTaskByTaskId(t);

            if (tTask.IsCompleted)
            {
                return t.Result;
            }

            var routeValues = new RouteValueDictionary
            {
                { "id", t.Id }
            };
            var retryafter = TimeSpan.FromSeconds(10).ToString();
            HttpContext.Response.Headers.RetryAfter = new string[]{ retryafter };
            return new RedirectToRouteResult(nameof(RouteConstants.GetTasksById), routeValues, permanent: true, preserveMethod: false);
        }
    }
}
