using Mockups.Models.Orders;
using Mockups.Repositories.Orders;
using Mockups.Services.Addresses;
using Mockups.Services.Carts;
using Mockups.Services.Users;
using Mockups.Storage;
using Mockups.Configs;
using Mockups.Services.MenuItems;
using System.Globalization;
using Mockups.Models.OrdersManagement;
using Mockups.Models.Cart;

namespace Mockups.Services.Orders
{
    public class OrdersService : IOrdersService
    {
        private readonly OrdersRepository _ordersRepository;
        private readonly ICartsService _cartsService;
        private readonly IUsersService _usersService;
        private readonly IAddressesService _addressesService;
        private readonly OrderConfig _orderTimeParams;
        private readonly IMenuItemsService _menuItemsService;

        public OrdersService(OrdersRepository ordersRepository, ICartsService cartsService, IUsersService usersService, IAddressesService addressesService, OrderConfig orderTimeParams, IMenuItemsService menuItemsService)
        {
            _ordersRepository = ordersRepository;
            _cartsService = cartsService;
            _usersService = usersService;
            _addressesService = addressesService;
            _orderTimeParams = orderTimeParams;
            _menuItemsService = menuItemsService;
        }

        public async Task CreateOrder(OrderCreatePostViewModel model, Guid userId)
        {
            var orderTime = DateTime.Now;

            var cartItems = (await _cartsService.GetUsersCart(userId)).Items;
            var price = 0f;
            foreach (var item in cartItems)
            {
                var itemPrice = (await _menuItemsService.GetItemModelById(item.Id.ToString())).Price * item.Amount;
                price += itemPrice;
            }

            var discount = 0f;
            var discountDescription = "";

            var userDOB = (await _usersService.GetUserInfo(userId)).BirthDate;
            var now = DateTime.Now;
            userDOB.AddYears(now.Year - userDOB.Year);
            if (Math.Abs((now - userDOB).Days) <= 3)//compare dates
            {
                discount = 15f;
                discountDescription = "На ваш заказ предоставляется скидка 15% в честь дня рождения!";
            }
            else if (now.Hour >= 11 && now.Hour < 15)//order time
            {
                discount = 10f;
                discountDescription = "На ваш заказ предоставляется скидка на ланч в 10%";
            }


            var order = new Order
            {
                CreationTime = orderTime,
                DeliveryTime = model.DeliveryTime,
                Cost = price,
                Discount = discount,
                Address = model.Address,
                Status = OrderStatus.New,
                UserId = userId,
            };

            var orderId = await _ordersRepository.AddOrder(order);

            foreach (var item in cartItems)
            {
                await _ordersRepository.AddOrderMenuItem(new OrderMenuItem
                {
                    OrderId = orderId,
                    ItemId = item.Id,
                    Amount = item.Amount
                });
            }

            await _ordersRepository.Save();

            _cartsService.ClearUsersCart(userId);
        }

        public async Task EditOrder(OrderEditPostViewModel model)
        {
            var order = await _ordersRepository.GetOrderById(model.orderId);

            if (order == null)
            {
                throw new KeyNotFoundException();
            }

            order.Status = model.Status;
            order.DeliveryTime = model.DeliveryTime;

            await _ordersRepository.EditOrder(order);
        }

        public async Task<OrdersManagementIndexViewModel> GetAllOrders()
        {
            var orders = await _ordersRepository.GetAllOrders();
            orders.Sort(new OrderComparer());

            var orderVMs = new List<OrderShortViewModel>();
            foreach (var order in orders)
            {
                var orderDate = order.CreationTime.ToString("MM.dd.yyyy HH:mm");
                var orderStatus = order.Status.GetDisplayName();
                var orderInfo = (order.Status == OrderStatus.Delivered ? "Заказ доставлен " : "Заказ ожидается ") + order.DeliveryTime.ToString("MM.dd.yyyy HH:mm");
                var orderItems = await _ordersRepository.GetOrdersItems(order.Id);
                var orderContents = string.Join(", ", orderItems.Select(x => x.Item.Name).ToList());

                orderVMs.Add(new OrderShortViewModel
                {
                    Id = order.Id,
                    Date = orderDate,
                    Status = orderStatus,
                    StatusInfo = orderInfo,
                    Contents = orderContents
                });
            }

            return new OrdersManagementIndexViewModel
            {
                Items = orderVMs
            };
        }

        public async Task<OrderCreateWrapperViewModel> GetCreateOrderViewModel(Guid userId)
        {
            var cartItems = (await _cartsService.GetUsersCart(userId, true)).Items;

            var addresses = _addressesService.GetAddressesByUserId(userId);
            var addressStrings = new List<string>();
            addressStrings.Add(addresses.Where(a => a.IsMainAddress == true).First().GetAddressString());
            foreach (var address in addresses)
            {
                if (!addressStrings.Contains(address.GetAddressString()))
                {
                    addressStrings.Add(address.GetAddressString());
                }
            }

            //System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo(1053);

            var deliveryTimes = new List<DateTime>();
            for (int i = 0; i < 5; i++)
            {
                var time = DateTime.Now.AddMinutes(_orderTimeParams.MinDeliveryTime).AddMinutes(i * _orderTimeParams.DeliveryTimeStep);
                deliveryTimes.Add(time);
            }

            var price = 0f;
            foreach (var item in cartItems)
            {
                var itemPrice = (await _menuItemsService.GetItemModelById(item.Id.ToString())).Price * item.Amount;
                price += itemPrice;
            }

            var discount = 0f;
            var discountDescription = "";

            var userDOB = (await _usersService.GetUserInfo(userId)).BirthDate;
            var now = DateTime.Now;
            userDOB = userDOB.AddYears(now.Year - userDOB.Year).AddHours(now.Hour).AddMinutes(now.Minute).AddSeconds(now.Second + 1);
            if (Math.Abs((now - userDOB).Days) <= 3)//compare dates
            {
                discount = 15f;
                discountDescription = "На ваш заказ предоставляется скидка 15% в честь дня рождения!";
            }
            else if (now.Hour >= 11 && now.Hour < 15)//order time
            {
                discount = 10f;
                discountDescription = "На ваш заказ предоставляется скидка на ланч в 10%";
            }

            return new OrderCreateWrapperViewModel
            {
                GetModel = new OrderCreateGetViewModel
                {
                    Items = cartItems,
                    Addresses = addressStrings,
                    DeliveryTimeOptions = deliveryTimes,
                    Price = price,
                    Discount = discount,
                    DiscountDescription = discountDescription
                },
                PostModel = new OrderCreatePostViewModel()
            };
        }

        public async Task<OrderEditViewModel> GetEditModel(int orderId)
        {
            var order = await _ordersRepository.GetOrderById(orderId);

            if (order == null)
            {
                throw new KeyNotFoundException();
            }

            return new OrderEditViewModel
            {
                GetModel = new OrderEditGetViewModel
                {
                    Id = order.Id,
                    CreationTime = order.CreationTime.ToString("MM.dd.yyyy HH:mm"),
                    DeliveryTime = order.DeliveryTime,
                    CurrentStatus = order.Status,
                    NextStatus = order.Status.GetNextStatus()
                },
                PostModel = new OrderEditPostViewModel()
            };
        }

        public async Task<OrderInfoViewModel> GetOrderInfo(int orderId)
        {
            var order = await _ordersRepository.GetOrderById(orderId);

            if (order == null)
            {
                throw new KeyNotFoundException();
            }


            var orderItems = await _ordersRepository.GetOrdersItems(orderId);
            var contents = new List<CartMenuItemViewModel>();

            foreach (var item in orderItems)
            {
                contents.Add(new CartMenuItemViewModel
                {
                    Id = item.Item.Id,
                    Name = item.Item.Name,
                    Amount = item.Amount
                });
            }

            return new OrderInfoViewModel
            {
                Id = order.Id,
                CreationTime = order.CreationTime.ToString("MM.dd.yyyy HH:mm"),
                Status = order.Status.GetDisplayName(),
                StatusInfo = (order.Status == OrderStatus.Delivered ? "Заказ доставлен " : "Заказ ожидается ") + order.DeliveryTime.ToString("MM.dd.yyyy HH:mm"),
                TotalCost = order.Cost * (100 - order.Discount) / 100,
                Address = order.Address,
                Items = contents
            };
        }

        public async Task<OrderIndexViewModel> GetPastOrders(Guid userId)
        {
            var orders = await _ordersRepository.GetUsersPastOrders(userId);

            var orderVMs = new List<OrderShortViewModel>();
            foreach (var order in orders)
            {
                var orderDate = order.CreationTime.ToString("MM.dd.yyyy HH:mm");
                var orderStatus = order.Status.GetDisplayName();
                var orderInfo = (order.Status == OrderStatus.Delivered ? "Заказ доставлен " : "Заказ ожидается ") + order.DeliveryTime.ToString("MM.dd.yyyy HH:mm");
                var orderItems = await _ordersRepository.GetOrdersItems(order.Id);
                var orderContents = string.Join(", ", orderItems.Select(x => x.Item.Name).ToList());

                orderVMs.Add(new OrderShortViewModel
                {
                    Id = order.Id,
                    Date = orderDate,
                    Status = orderStatus,
                    StatusInfo = orderInfo,
                    Contents = orderContents
                });
            }

            orderVMs.Reverse();

            var indexVM = new OrderIndexViewModel
            {
                Orders = orderVMs,
                CartIsEmpty = _cartsService.GetCartItemCount(userId) == 0
            };

            return indexVM;
        }
    }




    public class OrderComparer : IComparer<Order>
    {
        public int Compare(Order? x, Order? y)
        {
            if (x == null && y == null)
            {
                return 0;
            }
            else if (x == null)
            {
                return 1;
            }
            else if (y == null)
            {
                return -1;
            }

            if (x.Status == OrderStatus.New && y.Status != OrderStatus.New)
            {
                return -1;
            }
            if (y.Status == OrderStatus.New && x.Status != OrderStatus.New)
            {
                return 1;
            }

            if (x.DeliveryTime > y.DeliveryTime)
            {
                return 1;
            }
            else if (x.DeliveryTime < y.DeliveryTime)
            {
                return -1;
            }

            if (x.Status < y.Status)
            {
                return -1;
            }
            else if (x.Status > y.Status)
            {
                return 1;
            }


            return 0;
        }
    }
}
