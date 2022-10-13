using Mockups.Models.Cart;
using Mockups.Services.Carts;
using Mockups.Services.MenuItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Mockups.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IMenuItemsService _menuItemsService;
        private readonly ICartsService _cartsService;

        public CartController(IMenuItemsService menuItemsService, ICartsService cartsService)
        {
            _menuItemsService = menuItemsService;
            _cartsService = cartsService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

            var model = await _cartsService.GetUsersCart(userId);

            return View(model);
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemId = Guid.Parse(id);
            var userId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var cart = await _cartsService.GetUsersCart(userId);

            if (!cart.Items.Any(x => x.Id == itemId))
            {
                return RedirectToAction("Index", "Cart");
            }

            //_cartsService.DeleteItemFromCart(userId, itemId);

            var itemName = await _menuItemsService.GetItemNameById(itemId);

            if (itemName == null)
            {
                return NotFound();
            }

            var model = new DeleteFromCartViewModel
            {
                Id = itemId,
                Name = itemName
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemId = Guid.Parse(id);
            var userId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var cart = await _cartsService.GetUsersCart(userId);

            if (!cart.Items.Any(x => x.Id == itemId))
            {
                return RedirectToAction("Index", "Cart");
            }

            _cartsService.DeleteItemFromCart(userId, itemId);

            return RedirectToAction("Index", "Cart");
        }
    }
}
