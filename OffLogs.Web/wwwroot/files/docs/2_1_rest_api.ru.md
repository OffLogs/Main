## Rest API

Это базовое API которое может быть использовано для отправки информации(логов).
Если вы не используете клиенты OffLogs, в этом случае, вы можете создать свой клиент который будет
отправлять информацию на https:://api.offlogs.com

### Пример запроса

> URL: **https://api.offlogs.com/log/add**

> HTTP Request Type: **POST**

> Content-Type: **application/json**

> Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9zeXN0ZW0iOiJBcHBsaWNhdGlvbiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiMjYiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9yc2EiOiJNSUlCSWpBTkJna3Foa2lHOXcwQkFRRUZBQU9DQVE4QU1JSUJDZ0tDQVFFQXhGMktaMWNoTUVDQUx2WTZLdkpFKzdkOW9oaFFCam9zdmVRRDhCTTJvVGJtM3h6NWxLaE0rZjJZSk9zSEI1QTduV0IzWlhaM3RQS2RleitGOHYyd2RoWEhuZ25VU1lwNG9DdFZmMmZ3T2RWdXRnbkcybXIrUmpQNTc2Q1llSVMrQlBuQVFwSEJmNTJJZDJienVyTU9PbFF1VERnRVBDcWdrZDkralZVNmZtbWZGQWNMSGw2TUNqTWNSLzh5YW9oRmtQRjhkVFJVWC9HTlZJSDBlMUZtNkRnSzNwL3ptRE5YZ0hMT0M5bGNXZ09oaVhvRWN6ZUI5NmNhdU9GOG54cGJwNVJ4YjZtMjVsQUFPWGlOdXJuUFVrVjA0MDlHZ05KOTdXTGlub2xrTitMMzNKZlBYVDNhamdTN3lRZ01CMkpVem9pZkdxME5ZQ1RSOWFzL0JpQ1JUd0lEQVFBQiIsImlzcyI6Ik9mZkxvZ3MgQXBwbGljYXRpb24gQVBJIiwiYXVkIjoiT2ZmTG9ncyBBcHBsaWNhdGlvbiBBUEkifQ.MACdClRtkUS4rTcakJ_u3mjRFom--ESYjti01Ol78Pk

Body:
```json
{
    "logs": [
        {
            "Timestamp": "2022-04-09T08:41:07Z",
            "Level": "I",
            "Properties": {
                "property1": "value1"
            }
        },
        {
            "Timestamp": "2022-04-09T08:41:07Z",
            "Level": "E",
            "Traces": [
                "trace1",
                "trace2"
            ]
        }
    ]
}
```

### Параметры

> #### Level:
> Type: enum
> 
> Больше информации можно найти по [ссылке]()

Timestamp: ISO 8601
