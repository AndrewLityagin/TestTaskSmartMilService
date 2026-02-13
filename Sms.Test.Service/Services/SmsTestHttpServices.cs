using System.Net.Http.Json;
using Sms.Test.Service.DTO;

namespace Sms.Test.Service;

public class SmsTestHttpServices: IOrderService
{
    private readonly HttpClient _httpClient;
    
    public SmsTestHttpServices(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<MenuItemDto[]> GetMenu (bool withPrice)
    {
        var request = new
        {
            Command = "GetMenu",
            CommandParameters = new
            {
                WithPrice = withPrice
            }
        };
        
        var rawResponse =  await _httpClient.PostAsJsonAsync("",request);
        var getMenuResponse = await rawResponse.Content.ReadFromJsonAsync<GetMenuResponseDto>();
        
        if (getMenuResponse == null)
            return Array.Empty<MenuItemDto>();
        
        if (!getMenuResponse.Success)
            throw new InvalidOperationException(getMenuResponse.ErrorMessage);
        
        return getMenuResponse.MenuItems;
    }
    
    public async Task<bool> SendOrder(OrderDto orderDto)
    {
        var request = new
        {
            Command = "SendOrder",
            CommandParameters = new
            {
                OrderId =  orderDto.Id,
                MenuItems = orderDto.OrderItems
            }
        };
        var rawResponse =  await _httpClient.PostAsJsonAsync("",request);
        var sendOrderResponse = await rawResponse.Content.ReadFromJsonAsync<SendOrderResponseDto>();
    
        if (sendOrderResponse == null)
            return false;
        
        if (!sendOrderResponse.Success)
            throw new InvalidOperationException(sendOrderResponse.ErrorMessage);
        
        return true;
    }
}