using Sms.Test.Service;
using Sms.Test.Service.DTO;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var httpBaseUrl = builder.Configuration["SmsTest:HttpBaseUrl"];
if (string.IsNullOrWhiteSpace(httpBaseUrl))
{
    throw new InvalidOperationException("Configuration value 'SmsTest:HttpBaseUrl' is required.");
}

var grpcAddress = builder.Configuration["SmsTest:GrpcAddress"];
if (string.IsNullOrWhiteSpace(grpcAddress))
{
    throw new InvalidOperationException("Configuration value 'SmsTest:GrpcAddress' is required.");
}

builder.Services.AddHttpClient<SmsTestHttpServices>(client => { client.BaseAddress = new Uri(httpBaseUrl); });

builder.Services.AddGrpcClient<SmsTestService.SmsTestServiceClient>(options =>
{
    options.Address = new Uri(grpcAddress);
});

builder.Services.AddScoped<SmsTestGrpcService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/sms/http/menu", async (bool withPrice, SmsTestHttpServices service) =>
{
    try
    {
        return Results.Ok(await service.GetMenu(withPrice));
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.MapGet("/api/sms/grpc/menu", async (bool withPrice, SmsTestGrpcService service) =>
{
    try
    {
        return Results.Ok(await service.GetMenu(withPrice));
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.MapPost("/api/sms/http/order", async (OrderDto order, SmsTestHttpServices service) =>
{
    try
    {
        return Results.Ok(new { sent = await service.SendOrder(order) });
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.MapPost("/api/sms/grpc/order", async (OrderDto order, SmsTestGrpcService service) =>
{
    try
    {
        return Results.Ok(new { sent = await service.SendOrder(order) });
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.Run();
