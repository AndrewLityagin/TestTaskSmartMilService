namespace Sms.Test.Service.DTO;

public class GetMenuResponseDto
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
    public MenuData Data {get;set;}
    
}
public  class MenuData
{
 public  MenuItemDto[] MenuItems {get;set;}
}