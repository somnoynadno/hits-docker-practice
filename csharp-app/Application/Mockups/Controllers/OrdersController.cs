using Mockups.Models.Orders;
using Mockups.Services.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Mockups.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrdersService _ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

            var model = await _ordersService.GetPastOrders(userId);

            return View(model);
        }

        [HttpGet]
        [ActionName("Create")]
        public async Task<IActionResult> CreateGet()
        {
            var userId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

            var model = await _ordersService.GetCreateOrderViewModel(userId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public async Task<IActionResult> CreatePost(OrderCreateWrapperViewModel model)
        {
            var userId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

            await _ordersService.CreateOrder(model.PostModel, userId);

            return RedirectToAction("Index");
        }
    }
}
