using Mockups.Models.Orders;
using Mockups.Models.OrdersManagement;

namespace Mockups.Services.Orders
{
    public interface IOrdersService
    {
        Task<OrderIndexViewModel> GetPastOrders(Guid userId);
        Task CreateOrder(OrderCreatePostViewModel model, Guid userId);
        Task<OrderCreateWrapperViewModel> GetCreateOrderViewModel(Guid userId);
        Task<OrdersManagementIndexViewModel> GetAllOrders();
        Task<OrderInfoViewModel> GetOrderInfo(int orderId);
        Task<OrderEditViewModel> GetEditModel(int orderId);
        Task EditOrder(OrderEditPostViewModel model);
    }
}
