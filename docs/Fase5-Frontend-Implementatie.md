# Eternal Kids – Fase 5 (Frontend Implementatie)

## Opgeleverd

- `/Prijzen` Razor Page (`Pages/Prijzen/Index`) met dynamische package cards (Basic/Elegant/Prestige/Deluxe) en prijsberekening via `IPricingService`.
- `/Planner` Razor Page (`Pages/Planner/Index`) met luxe chat-UI en SignalR integratie naar `ChatHub`.
- Winkelwagen partial component (`_CartSummary`) in de topnavigatie met itemcount en subtotaal.
- `_Layout.cshtml` bijgewerkt met nieuwe navigatie-items `Prijzen` en `AI-Tool` (Planner route), plus cart summary integratie.

## Interactie

- Prijzenpagina ondersteunt eventtype + guestcount selectie en server-side prijsophaal per pakket.
- Planner stuurt realtime berichten via SignalR (`JoinSession`, `SendMessage`) en rendert assistant antwoorden direct in de chat.
