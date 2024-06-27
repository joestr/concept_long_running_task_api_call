using concept_long_running_task_api_call.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace concept_long_running_task_api_call
{
    public class OrderPbdLayer
    {
        List<Order> orders = new List<Order>();

        private TaskPbdLayer taskPbdLayer = null;

        public OrderPbdLayer(TaskPbdLayer taskPbdLayer)
        {
            this.taskPbdLayer = taskPbdLayer;
        }

        public Task NewOrder(string name)
        {
            Task task = null;

            task = taskPbdLayer.NewTask(() =>
            {
                Order order = new Order();

                order.Id = Guid.NewGuid();
                Thread.Sleep(20000);

                orders.Add(order);

                var routeValues = new RouteValueDictionary
                {
                    { "id", order.Id }
                };
                task.Result = new RedirectToRouteResult(nameof(RouteConstants.GetOrdersById), routeValues, permanent: true, preserveMethod: false);
            });


            taskPbdLayer.GetTTaskByTaskId(task).Start();

            return task;
        }

        public Order GetOrderById(Guid id)
        {
            return orders.Single(x => x.Id == id);
        }
    }
}
