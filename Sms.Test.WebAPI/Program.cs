using Sms.Test.WebAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<SmsTestGrpcService>();

app.MapGet("/", () => "Use gRPC client to communicate with this server.");

app.Run();

/*
//Test HTTP
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Чтобы оставить PascalCase (Command, Success и т.д.)
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = null;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/api/command", (JsonElement json) =>
{
    Console.WriteLine(json.ToString());
    var command = json.GetProperty("command").GetString();

    if (command == "GetMenu")
    {
        var request = json.Deserialize<GetMenuRequest>();
        return Results.Ok(HandleGetMenu(request!));
    }

    if (command == "SendOrder")
    {
        var request = json.Deserialize<SendOrderRequest>();
        return Results.Ok(HandleSendOrder(request!));
    }

    return Results.BadRequest();
})
.WithName("CommandHandler")
.WithOpenApi();

app.Run();


// ================== ЛОГИКА ==================

object HandleGetMenu(BaseRequest request)
{
    //var getMenuRequest = request as GetMenuRequest;

    var menuItems = new List<MenuItem>
    {
        new MenuItem
        {
            Id = "5979224",
            Article = "A1004292",
            Name = "Каша гречневая",
            Price = 50,//getMenuRequest?.CommandParameters?.WithPrice == true ? 50 : null,
            IsWeighted = false,
            FullPath = "ПРОИЗВОДСТВО\\Гарниры",
            Barcodes = new List<string> { "57890975627974236429" }
        },
        new MenuItem
        {
            Id = "9084246",
            Article = "A1004293",
            Name = "Конфеты Коровка",
            Price = 300, //getMenuRequest?.CommandParameters?.WithPrice == true ? 300 : null,
            IsWeighted = true,
            FullPath = "ДЕСЕРТЫ\\Развес",
            Barcodes = new List<string>()
        }
    };

    return new GetMenuResponse
    {
        Command = "GetMenu",
        Success = true,
        ErrorMessage = "",
        Data = new MenuData
        {
            MenuItems = menuItems
        }
    };
}

object HandleSendOrder(BaseRequest request)
{
    var sendOrderRequest = request as SendOrderRequest;

    // Здесь может быть логика сохранения заказа в БД

    return new BaseResponse
    {
        Command = "SendOrder",
        Success = true,
        ErrorMessage = ""
    };
}


// ================== DTO ==================

public class BaseRequest
{
    public string Command { get; set; } = string.Empty;
}

public class BaseResponse
{
    public string Command { get; set; } = string.Empty;
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
}


// ---------- GetMenu ----------

public class GetMenuRequest : BaseRequest
{
    public GetMenuParameters? CommandParameters { get; set; }
}

public class GetMenuParameters
{
    public bool WithPrice { get; set; }
}

public class GetMenuResponse : BaseResponse
{
    public MenuData? Data { get; set; }
}

public class MenuData
{
    public List<MenuItem>? MenuItems { get; set; }
}

public class MenuItem
{
    public string Id { get; set; } = string.Empty;
    public string Article { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int? Price { get; set; }
    public bool IsWeighted { get; set; }
    public string FullPath { get; set; } = string.Empty;
    public List<string> Barcodes { get; set; } = new();
}


// ---------- SendOrder ----------

public class SendOrderRequest : BaseRequest
{
    public SendOrderParameters? CommandParameters { get; set; }
}

public class SendOrderParameters
{
    public string OrderId { get; set; } = string.Empty;
    public List<OrderItem>? Items { get; set; }
}

public class OrderItem
{
    public string Id { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
}
*/
