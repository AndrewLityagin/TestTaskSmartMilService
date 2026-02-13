using System.Reflection.Metadata.Ecma335;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Sms.Test.Service.DTO;

namespace Sms.Test.Service;

public class SmsTestGrpcService: IOrderService
{
    private readonly SmsTestService.SmsTestServiceClient _client;
    
    public SmsTestGrpcService(
        SmsTestService.SmsTestServiceClient client) 
    {
        _client = client;
    }
    
    public async Task<MenuItemDto[]> GetMenu(bool withPrice)
    {
        var reply = await _client.GetMenuAsync(new BoolValue{Value = withPrice});
        
        if (reply == null)
            return Array.Empty<MenuItemDto>();
        
        if (!reply.Success)
            throw new InvalidOperationException(reply.ErrorMessage);

        return reply.MenuItems.Select(mi => Mapper.ToDto(mi)).ToArray();
    }   
    
    public async Task<bool> SendOrder(OrderDto order)
    {
        var reply = await _client.SendOrderAsync(Mapper.FromDto(order));

        if (reply == null)
            return false;
        
        if (!reply.Success)
            throw new InvalidOperationException(reply.ErrorMessage);

        return true;
    }
}