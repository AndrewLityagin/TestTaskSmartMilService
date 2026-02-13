namespace Sms.Test.Service.DTO;

public class OrderDto
{
   public string Id { get; set; } = Guid.NewGuid().ToString();
   public OrderItemDto[] OrderItems { get; set; }
}