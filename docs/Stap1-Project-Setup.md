# Eternal Kids – Fase 1 (Project Initialisatie & Layout)

## 1) Visual Studio 2022 project-setup (ASP.NET Core Razor Pages)

> Doel: een enterprise-ready basis neerzetten met naamgeving **Eternal Kids**.

1. Open **Visual Studio 2022**.
2. Kies **Create a new project**.
3. Kies template **ASP.NET Core Web App (Razor Pages)**.
4. Projectnaam: `EternalKids.Web`.
5. Solutionnaam: `EternalKids`.
6. Locatie: bijvoorbeeld `src/` in de repository.
7. Framework: kies de beschikbare .NET versie in VS (bij voorkeur **Latest / .NET 8 of hoger**).
8. Authentication: **Individual Accounts** (enterprise-ready met ASP.NET Core Identity).
9. Vink aan:
   - **Configure for HTTPS**
   - **Enable Razor runtime compilation** (optioneel voor snelle UI-iteratie)
10. Klik **Create**.

### Aanbevolen solution-structuur (fase 1)

- `EternalKids.sln`
- `src/EternalKids.Web` (Razor Pages host)
- `src/EternalKids.Application` (services/use-cases)
- `src/EternalKids.Domain` (entities + business rules)
- `src/EternalKids.Infrastructure` (EF Core, mail, payment, AI clients)
- `tests/EternalKids.Web.Tests`
- `tests/EternalKids.Application.Tests`

## 2) NuGet-pakketten voor enterprise-opzet

Installeer deze packages in `EternalKids.Web` (en waar relevant in Infrastructure/Application).

### Data & EF Core
- `Microsoft.EntityFrameworkCore`
- `Microsoft.EntityFrameworkCore.SqlServer`
- `Microsoft.EntityFrameworkCore.Design`
- `Microsoft.EntityFrameworkCore.Tools`

### Identity, security & auth
- `Microsoft.AspNetCore.Identity.EntityFrameworkCore`
- `Microsoft.AspNetCore.Authentication.Google` (optioneel, social login)
- `Microsoft.AspNetCore.Authentication.JwtBearer` (voor API endpoints/hubs indien nodig)
- `Microsoft.AspNetCore.DataProtection.EntityFrameworkCore`

### Observability & logging
- `Serilog.AspNetCore`
- `Serilog.Sinks.Console`
- `Serilog.Sinks.File`
- `Serilog.Sinks.Seq` (optioneel)

### Validatie & mapping
- `FluentValidation.AspNetCore`
- `Mapster` of `AutoMapper.Extensions.Microsoft.DependencyInjection`

### Payments (kies 1 provider)
- `Mollie.Api`
- `Stripe.net`

### AI tooling
- `Microsoft.SemanticKernel`
- `Azure.AI.OpenAI` (of `OpenAI` voor directe OpenAI integratie)

### Realtime chatbot
- `Microsoft.AspNetCore.SignalR.Client` (voor rich client scenario’s)

### E-mail
- `MailKit`

### Background jobs
- `Hangfire.Core`
- `Hangfire.AspNetCore`
- `Hangfire.SqlServer`

### API docs / helpers
- `Swashbuckle.AspNetCore` (voor interne API endpoints)

### Testing
- `xunit`
- `xunit.runner.visualstudio`
- `FluentAssertions`
- `Microsoft.AspNetCore.Mvc.Testing`

## 3) Branding- en namingrichtlijn (vanaf dag 1)

Gebruik consistent:
- Namespaces: `EternalKids.*`
- DbContext: `EternalKidsContext`
- CSS classes met prefix waar zinvol, bv. `ek-...`
- Config sections: `EternalKids:...`
- Database-objecten: `EternalKids_*`

## 4) Eerste navigatie (fase 1)

De layout bevat direct de kernroutes:
- Home
- Diensten
- AI-Tool
- Winkelwagen

Zie `src/EternalKids.Web/Pages/Shared/_Layout.cshtml`.
