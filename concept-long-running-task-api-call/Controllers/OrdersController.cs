using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace concept_long_running_task_api_call.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private OrderPbdLayer orderPbdLayer = null;

        public OrdersController(OrderPbdLayer orderPbdLayer)
        {
            this.orderPbdLayer = orderPbdLayer;
        }

        [HttpPost]
        [Route("", Name = nameof(RouteConstants.PostOrders))]
        public ActionResult CreateOrder()
        {
            var orderTask = orderPbdLayer.NewOrder("");
            var routeValues = new RouteValueDictionary
            {
                { "id", orderTask.Id }
            };
            return new AcceptedAtRouteResult(nameof(RouteConstants.GetTasksById), routeValues, null);
        }

        [HttpGet]
        [Route("{id}", Name = nameof(RouteConstants.GetOrdersById))]
        public ActionResult GetOrder([FromRoute] Guid id)
        {
            Order order;

            try
            {
                order = orderPbdLayer.GetOrderById(id);
            }
            catch (InvalidOperationException)
            {
                return new NotFoundResult();
            }

            return new JsonResult(order);
        }
    }
}
