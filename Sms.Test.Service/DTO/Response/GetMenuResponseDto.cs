namespace Sms.Test.Service.DTO;

public class GetMenuResponseDto
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
    public MenuItemDto[] MenuItems { get; set; }
}