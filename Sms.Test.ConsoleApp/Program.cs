using System.Net.Http.Headers;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sms.Test.ConsoleApp;
using Sms.Test.Service;
using Sms.Test.Service.DTO;
using Serilog;

var host = Host.CreateDefaultBuilder(args)
    .UseSerilog()
    .ConfigureAppConfiguration((context, config) =>
    {
        config.SetBasePath(Directory.GetCurrentDirectory());
        config.AddJsonFile("appsettings.json", optional: false);
    })
    .ConfigureServices((context, services) =>
    {
        var config = context.Configuration;
        
        services.AddHttpClient<SmsTestHttpServices>(client =>
        {
            var httpUrl = config["Sms:HttpUrl"];
            
            if(string.IsNullOrEmpty(httpUrl))
                throw new InvalidProgramException("Invalid configuration: HttpUrl");
            
            client.BaseAddress = new Uri(httpUrl);
            
            var username = config["Sms:Username"];
            var password = config["Sms:Password"];

            var credentials = Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{username}:{password}")
            );

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", credentials);

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        });
        
        var grpcUrl = config["Sms:GrpcUrl"];
            
        if(string.IsNullOrEmpty(grpcUrl))
            throw new InvalidProgramException("Invalid configuration: GrpcUrl");
        
        services.AddGrpcClient<SmsTestService.SmsTestServiceClient>(options =>
        {
            options.Address = new Uri(grpcUrl);
        });
        
        /*
         * Комментарий #1:
         * Согласно Заданию 1 Часть 2  нужно оставить активной реализацию через gRPC
         * Поэтому в DI добавил реализацию IOrderService как SmsTestGrpcService
         */
        services.AddScoped<IOrderService,SmsTestGrpcService>();
        
        
        var connectionString = config.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidProgramException("Invalid configuration: ConnectionStrings:DefaultConnection");
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        var logName = $"test-sms-console-app-.log";
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File(
                path: logName,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7)
            .CreateLogger();
        
    })
    .Build();


using var scope = host.Services.CreateScope();

var logger = scope.ServiceProvider
    .GetRequiredService<ILogger<Program>>();

var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
if (!await db.Database.CanConnectAsync())
{
    logger.LogError("Cannot connect to database");
    return;
}
await db.Database.EnsureCreatedAsync();

try
{
    var orderService =  scope.ServiceProvider.GetRequiredService<IOrderService>();
    //2-3
    var menuItems = (await orderService.GetMenu(true)).ToArray();
    db.MenuItems.AddRange(menuItems);
    db.SaveChanges();
    logger.LogInformation($"Название – Код (артикул) – Цена за единицу");
    foreach (var item in menuItems)
        logger.LogInformation(item.ToString());
    //4
    var order = new OrderDto();
    var attempts = 0;
    OrderItemDto[] orderItems;
    do
    {
        orderItems = GetOrderItems();
        attempts++;
        if (orderItems.Length > 0 && CheckOrderItems(orderItems, menuItems))
            break;
        else
        {
            orderItems = [];
            logger.LogError("Incorrect input. Please try one more time!");
        }
    } while (attempts > 5);
    
    if (orderItems.Length == 0)
        throw new InvalidProgramException("Incorrect input. All attempts are used");

    order.OrderItems = orderItems;
    try
    {
        var sendOrderResult = await orderService.SendOrder(order);
        if (sendOrderResult)
            logger.LogInformation("УСПЕХ");
    }
    catch (InvalidOperationException ex)
    {
        logger.LogError("НЕУДАЧА : " + ex.Message);
    }
}
catch (Exception ex)
{
    logger.LogError(ex.Message);
    Environment.Exit(0);
}


OrderItemDto[] GetOrderItems()
{
    try
    {
        logger.LogInformation("Please enter order with format: Id:Quantity;Id:Quantity ... ");
        var line = Console.ReadLine() ?? "";
        var items = line.Split(';');
        var orderItems = items.Select(item => new OrderItemDto()
            { Id = item.Split(":")[0], Quantity = int.Parse(item.Split(":")[1]) });
        return orderItems.ToArray();
    }
    catch (Exception ex)
    {
        logger.LogError(ex.Message);
    }
    return [];
}

bool CheckOrderItems(OrderItemDto[] orderItems, MenuItemDto[] menuItems)
{
    try
    {
        foreach (var item in orderItems)
        {
            if (!menuItems.Any(mi => mi.Id == item.Id))
                return false;
            
            if (item.Quantity <= 0)
                return false;
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex.Message);
    }
    return true;
}