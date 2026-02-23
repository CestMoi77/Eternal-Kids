# Eternal Kids – Fase 4 (AI & Real-time communicatie)

## Geïmplementeerde onderdelen

### 1) AiConversationService
- Ondersteunt tool-calling met tool `get_price`.
- Roept `IPricingService` aan voor echte prijzen op basis van event/package/guest count.
- Schrijft chatberichten weg in `ChatMessage` (roles: User, Tool, Assistant).
- Schrijft AI request logs weg in `EternalKids_AI_Request_Log` via `AiRequestLog`.

### 2) ChatHub (SignalR)
- `JoinSession(sessionId)` voor group-based realtime sessies.
- `SendMessage(...)` routeert user-bericht naar `IAiConversationService`.
- Stuurt assistant antwoord realtime terug naar alle clients in de sessie.

## Nieuwe contracts/DTOs
- `IAiConversationService`
- `IAiModelClient`
- `AiConversationDtos` (tool-calls, tool-definities, model request/response, ui-actions)
