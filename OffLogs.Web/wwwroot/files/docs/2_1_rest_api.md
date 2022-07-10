## Rest API

Это базовое API которое может быть использовано для отправки информации(логов).
Если вы не используете клиенты OffLogs, в этом случае, вы можете создать свой клиент который будет
отправлять информацию на https:://api.offlogs.com

### HTTP request example

> URL: **https://api.offlogs.com/log/add**
>
> HTTP Request Type: **POST**
>
> Content-Type: **application/json**
>
> Authorization: **Bearer < Application API Token >**

> You can read about how to get the Application API Token [in this section](/documentation/common/1_3_applications).

### For test purposes, you can use the token below:
```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9zeXN0ZW0iOiJBcHBsaWNhdGlvbiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiMjYiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9yc2EiOiJNSUlCSWpBTkJna3Foa2lHOXcwQkFRRUZBQU9DQVE4QU1JSUJDZ0tDQVFFQXhGMktaMWNoTUVDQUx2WTZLdkpFKzdkOW9oaFFCam9zdmVRRDhCTTJvVGJtM3h6NWxLaE0rZjJZSk9zSEI1QTduV0IzWlhaM3RQS2RleitGOHYyd2RoWEhuZ25VU1lwNG9DdFZmMmZ3T2RWdXRnbkcybXIrUmpQNTc2Q1llSVMrQlBuQVFwSEJmNTJJZDJienVyTU9PbFF1VERnRVBDcWdrZDkralZVNmZtbWZGQWNMSGw2TUNqTWNSLzh5YW9oRmtQRjhkVFJVWC9HTlZJSDBlMUZtNkRnSzNwL3ptRE5YZ0hMT0M5bGNXZ09oaVhvRWN6ZUI5NmNhdU9GOG54cGJwNVJ4YjZtMjVsQUFPWGlOdXJuUFVrVjA0MDlHZ05KOTdXTGlub2xrTitMMzNKZlBYVDNhamdTN3lRZ01CMkpVem9pZkdxME5ZQ1RSOWFzL0JpQ1JUd0lEQVFBQiIsImlzcyI6Ik9mZkxvZ3MgQXBwbGljYXRpb24gQVBJIiwiYXVkIjoiT2ZmTG9ncyBBcHBsaWNhdGlvbiBBUEkifQ.MACdClRtkUS4rTcakJ_u3mjRFom--ESYjti01Ol78Pk
```

>
> Attention! You can only use this token to test an HTTP request and send test information.
> You will not be able to view the sent information in OffLogs, 
> for this you need to register and create an application with a new token
>

#### Request body example

In this example, only 2 logs of type Information and Error are used.
But you can send logs of different types in one request.

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

### Parameter List

> #### logs
> Тип: array
>
> An array of objects containing debug information.
>
> The maximum number of elements that can be contained in an array: **99**

> #### Level
> Тип: Enum
>
> Defines logging severity levels.
> You can find the list of possible values [here](/documentation/message_resource/3_1_log_level).

> #### Timestamp
> Тип: DateTime<ISO 8601>
>
> Time of creation or sending of logged information

> #### Properties
> Тип: Object<string, object>
>
> The parameter must contain key-value pairs with additional information, parameters.
> You can transfer any data you need

> #### Traces
> Тип: Array< string >
>
> Used to pass information about the execution stack.
