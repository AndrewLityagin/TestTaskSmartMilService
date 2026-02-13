using Sms.Test.Service.DTO;

namespace Sms.Test.Service;

public interface IOrderService
{
    Task<MenuItemDto[]> GetMenu(bool withPrice);
    Task<bool> SendOrder(OrderDto order);
}