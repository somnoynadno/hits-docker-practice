using Mockups.Models;
using Mockups.Models.Menu;
using Mockups.Services.Carts;
using Mockups.Services.MenuItems;
using Mockups.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Mockups.Controllers
{
    public class MenuController : Controller
    {
        private readonly IMenuItemsService _menuItemsService;
        private readonly ICartsService _cartsService;

        public MenuController(IMenuItemsService menuItemsService, ICartsService cartsService)
        {
            _menuItemsService = menuItemsService;
            _cartsService = cartsService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] MenuItemCategory[]? filterCategory, [FromQuery] bool? filterIsVegan = null)
        {
            var items = await _menuItemsService.GetAllMenuItems(filterIsVegan, filterCategory);

            return View(items);
        }

        [HttpGet]
        [Authorize(Roles = ApplicationRoleNames.Administrator)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = ApplicationRoleNames.Administrator)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateMenuItemViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                await _menuItemsService.CreateMenuItem(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        [ActionName("Delete")]
        [Authorize(Roles = ApplicationRoleNames.Administrator)]
        public async Task<IActionResult> DeleteGet(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _menuItemsService.GetItemModelById(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        [Authorize(Roles = ApplicationRoleNames.Administrator)]
        public async Task<IActionResult> DeletePost(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _menuItemsService.DeleteMenuItem(id);

            if (result == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [ActionName("AddToCart")]
        [Authorize]
        public async Task<IActionResult> AddToCart(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _menuItemsService.GetAddToCartModel(id);

            return View(model);
        }

        [HttpPost]
        [ActionName("AddToCart")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddToCartPost(string id, int amount)
        {

            var userId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

            await _menuItemsService.AddItemToCart(userId, id, amount);
            return RedirectToAction("Index", "Menu");
        }

    }
}