using Microsoft.EntityFrameworkCore;
using Sms.Test.Service.DTO;

namespace Sms.Test.ConsoleApp;

public class AppDbContext: DbContext
{
    public DbSet<MenuItemDto> MenuItems => Set<MenuItemDto>();
    public DbSet<OrderDto> Orders => Set<OrderDto>();
    
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    
}