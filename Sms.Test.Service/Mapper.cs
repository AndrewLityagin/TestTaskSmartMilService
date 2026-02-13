using Sms.Test.Service.DTO;

namespace Sms.Test.Service;

public static class Mapper
{
    public static MenuItemDto ToDto (MenuItem item)
    {
        return new MenuItemDto()
        {
            Id = item.Id,
            Name = item.Name,
            Article = item.Article,
            Barcodes = item.Barcodes.ToArray(),
            Price = item.Price,
            FullPath = item.FullPath,
            IsWeighted = item.IsWeighted
        };
    }
    
    public static MenuItem FromDto (MenuItemDto item)
    {
        var menuItem =  new MenuItem()
        {
            Id = item.Id,
            Name = item.Name,
            Article = item.Article,
            Price = item.Price,
            FullPath = item.FullPath,
            IsWeighted = item.IsWeighted
        };
        menuItem.Barcodes.AddRange(item.Barcodes);
        return menuItem;
    }
    
    public static OrderDto ToDto (Order item)
    {
        return new OrderDto()
        {
            Id = item.Id,
            OrderItems = item.OrderItems.Select(x => new OrderItemDto()
            {
                Id = x.Id,
                Quantity = x.Quantity
            }).ToArray()
        };
    }
    
    public static Order FromDto (OrderDto item)
    {
        var order = new Order();
        var orderItems = item.OrderItems.Select(x => new OrderItem()
        {
            Id = x.Id,
            Quantity = x.Quantity,
        });
        order.Id = item.Id;
        order.OrderItems.AddRange(orderItems);
        return order;
    }
}