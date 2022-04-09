## Rest API

Это базовое API которое может быть использовано для отправки логов,
а так же вы можете использовать его при создании своего OffLogs клиента.

LogLevel:
Error: E
Warning: W
Fatal: F
Information: I
Debug: D

Timestamp: ISO 8601

```json
{
    "logs": [
        {
            "Timestamp": "2022-04-09T08:41:07Z",
            "Level": "I",
            "Traces": [
                "trace1"
            ],
            "Properties": {
                "property1": "value1"
            }
        }
    ]
}
```
