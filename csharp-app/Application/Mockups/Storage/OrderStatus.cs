using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Mockups.Storage
{
    public enum OrderStatus
    {
        [Display(Name = "Новый")]
        New,
        [Display(Name = "В работе")]
        InProcess,
        [Display(Name = "Готов")]
        Ready,
        [Display(Name = "Доставлен")]
        Delivered
    }

    public static class OrderStatusHelper
    {
        public static string GetDisplayName(this OrderStatus enumValue)
        {
            return enumValue.GetType().GetMember(enumValue.ToString())
                   .First()
                   .GetCustomAttribute<DisplayAttribute>()
                   .Name;
        }

        public static OrderStatus? GetNextStatus(this OrderStatus status)
        {
            switch (status)
            {
                case OrderStatus.New:
                    return OrderStatus.InProcess;
                case OrderStatus.InProcess:
                    return OrderStatus.Ready;
                case OrderStatus.Ready:
                    return OrderStatus.Delivered;
                case OrderStatus.Delivered:
                    return null;
                default:
                    return null;
            }
        }
    }
}
