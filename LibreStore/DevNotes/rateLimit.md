### Lessons Learned
While adding code for rate limiting I had a couple of additional items

Added package to use rate limiting:
`dotnet add package AspNetCoreRateLimit`

### Logging Via appsettings.json
Add the following to log rate limiting info:
```
"Logging": {
  "LogLevel": {
    "Default": "Information",
    "Microsoft.AspNetCore.RateLimiting": "Debug"
  }
```

### You Can Exclude URLs From Rate Limiting
In the example, /api/health would be excluded - 0 indicates no limit

```
"IpRateLimiting": {
  "EnableEndpointRateLimiting": true,
  "StackBlockedRequests": false,
  "RealIpHeader": "X-Real-IP",
  "GeneralRules": [
    {
      "Endpoint": "*",
      "Period": "1m",
      "Limit": 100
    },
    {
      "Endpoint": "/api/health",
      "Period": "1m",
      "Limit": 0
    }
  ]
}
```

