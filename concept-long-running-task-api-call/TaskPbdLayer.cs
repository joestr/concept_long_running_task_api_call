namespace concept_long_running_task_api_call
{
    public class TaskPbdLayer
    {
        Dictionary<Task, System.Threading.Tasks.Task> tasks = new Dictionary<Task, System.Threading.Tasks.Task>();
        public TaskPbdLayer() { }

        public Task NewTask(Action a)
        {
            var task = new Task();
            task.Id = Guid.NewGuid();
            task.Result = null;

            var tTask = new System.Threading.Tasks.Task(a);

            this.tasks.Add(task, tTask);

            return task;
        }

        public Task GetTaskById(Guid id)
        {
            return tasks.Keys.Single(x => x.Id == id);
        }

        public System.Threading.Tasks.Task GetTTaskByTaskId(Task task)
        {
            return tasks[task];
        }
    }
}
