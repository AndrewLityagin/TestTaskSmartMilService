# Sms.Test.WebApi

Web API проект для проверки работы сервисов из `Sms.Test.Service` по двум каналам:

- HTTP (`SmsTestHttpServices`)
- gRPC (`SmsTestGrpcService`)

## Конфигурация

Настройте адреса в `appsettings.json`:

```json
"SmsTest": {
  "HttpBaseUrl": "https://localhost:7010/api/commands",
  "GrpcAddress": "https://localhost:7020"
}
```

## Доступные endpoints

- `GET /api/sms/http/menu?withPrice=true`
- `GET /api/sms/grpc/menu?withPrice=true`
- `POST /api/sms/http/order`
- `POST /api/sms/grpc/order`

Тело запроса для `POST` соответствует `OrderDto` из `Sms.Test.Service`.
