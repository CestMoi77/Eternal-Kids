# Eternal Kids – Fase 2 (Data Models & Database)

Deze fase implementeert de kern-entiteiten uit hoofdstuk 6 van het onderzoeksdocument met Eternal Kids naamgeving.

## Geïmplementeerde entiteiten

- `Cart`
- `CartItem` *(met `OptionsJson`)*
- `Order` *(met `DepositAmount` en `Channel`)*
- `OrderItem` *(met `OptionsJson`)*
- `Payment`
- `PaymentEvent`
- `ChatSession`
- `ChatMessage`
- `AiRequestLog` *(mapped naar tabel `EternalKids_AI_Request_Log`)*

## Belangrijke velden conform requirement

- Flexibele opties in JSON:
  - `CartItem.OptionsJson`
  - `OrderItem.OptionsJson`
- Aanbetaling:
  - `Order.DepositRequired`
  - `Order.DepositRate`
  - `Order.DepositAmount`
- Kanaalherkomst:
  - `Order.Channel` met enum-waarden `Web`, `AiPlanner`, `Admin`

## DbContext

`EternalKidsContext` bevat DbSets en Fluent API mapping naar tabellen met prefix `EternalKids_`.
