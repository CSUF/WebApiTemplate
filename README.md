# CSUF Web API Template

The skeleton for the web API.  

The logging infrastructure is implemented using `Serilog`.

There are no _database back-ends_ in this sample web API.




## Development Environment

### Minimum Requirement

* Microsoft Visual 2019 16.6 or later
* .NET Core SDK 3.1.301
* .NET Core Runtime 3.1.5  

You can download the latest .NET Core SDK and .NET Core Runtime [here](https://dotnet.microsoft.com/download/dotnet-core/3.1)


### Always use HTTPS/TLS Certificate
This web application template always use HTTPS, even in the development environment.
Please make sure your development environment (IIS Express/Kestrel) has been configured to use the trusted TLS certificate (even self-signed).


### Configuration

All configuration are saved in the `appSettings.json` inside the **User Secrets**.  This configuration file never be uploaded to the source control.



### Error Handling


This topic will be described in more detail in the different section later.


## NuGet Packages

```
+ install-package IdentityModel
+ install-package IdentityServer4.AccessTokenValidation

+ install-package Microsoft.AspNetCore.JsonPatch
+ install-package Microsoft.AspNetCore.Mvc.NewtonsoftJson

+ install-package Microsoft.Data.SqlClient
+ install-package Microsoft.EntityFrameworkCore.SqlServer

+ install-package Microsoft.Extensions.Configuration.Json

+ install-package Microsoft.IdentityModel.Protocols.OpenIdConnect

+ install-package Serilog.AspNetCore

+ install-package Serilog.Enrichers.AspnetcoreHttpcontext
+ install-package Serilog.Enrichers.Environment
+ install-package Serilog.Enrichers.Memory
+ install-package Serilog.Enrichers.Process
+ install-package Serilog.Enrichers.Thread

+ install-package Serilog.Exceptions

+ install-package Serilog.Formatting.Compact

+ install-package Serilog.Settings.Configuration
+ install-package Serilog.Sinks.Async
+ install-package Serilog.Sinks.Console
+ install-package Serilog.Sinks.Email
+ install-package Serilog.Sinks.MSSqlServer
+ install-package Serilog.Sinks.RollingFile
```
 