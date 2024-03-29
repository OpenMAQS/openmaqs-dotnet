# Updating from MAQS 7 to MAQS 8

## Namespace
The name 'OpenMAQS' is replacing 'Magenic' in all namespaces. 
### Example
```csharp
// Old namespaces
// using Magenic.Maqs.Utilities.Logging;

// New namespaces
using OpenMAQS.Maqs.Utilities.Logging;
```

## Config
The 'MagenicMaqs' section of the configuration is being renamed 'GlobalMaqs'.
### Example (XML)
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <configSections>
    <section name="GlobalMaqs" type="System.Configuration.NameValueSectionHandler" />
    <!-- OLD <section name="MagenicMaqs" type="System.Configuration.NameValueSectionHandler" />-->
  </configSections>
  <GlobalMaqs>
    <add key="WaitTime" value="1000" />
    <add key="Timeout" value="10000" />
    <add key="Log" value="OnFail" />
    <add key="LogLevel" value="INFORMATION" />
    <add key="LogType" value="TXT" />
  </GlobalMaqs>
  <!-- OLD <MagenicMaqs>
    <add key="WaitTime" value="1000" />
    <add key="Timeout" value="10000" />
    <add key="Log" value="OnFail" />
    <add key="LogLevel" value="INFORMATION" />
    <add key="LogType" value="TXT" />
  </MagenicMaqs>-->
</configuration>
```
### Example (JSON)
```json
{
  "GlobalMaqs": {
    "WaitTime": "100",
    "Timeout": "10000",
    "Log": "OnFail",
    "LogLevel": "INFORMATION",
    "LogType": "TXT",
    "UseFirstChanceHandler": "YES",
    "SkipConfigValidation": "NO",
  }
}
```

