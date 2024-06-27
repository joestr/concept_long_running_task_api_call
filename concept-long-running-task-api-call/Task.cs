using Microsoft.AspNetCore.Mvc;

namespace concept_long_running_task_api_call
{
    public class Task
    {
        public Guid Id { get; set; }
        public RedirectToRouteResult Result { get; set; }
    }
}
