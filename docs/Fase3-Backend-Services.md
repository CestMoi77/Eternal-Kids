# Eternal Kids – Fase 3 (Core Functionaliteit: Backend Services)

## Geïmplementeerd

### PricingService
- Berekent prijzen op basis van `EventType`, `PackageType` en `GuestCount` via `PriceRule`.
- Ondersteunt actieve prijsregels met gast-range (`MinGuests`/`MaxGuests`).
- Ondersteunt optionele `OverrideUnitPrice` voor beheer/admin scenario's.

### ShoppingCartService
- `GetOrCreateCartAsync` op basis van `anonymousTokenHash`.
- `AddItemAsync` gebruikt `IPricingService` voor server-side prijsberekening.
- Slaat flexibele keuzes op in `CartItem.OptionsJson`.
- `CalculateTotalsAsync` levert subtotal/discount/grand total + `DepositAmount` (50%).

## Extra domeinuitbreiding

- `PriceRule` entiteit toegevoegd (Prijzenbeheer-model).
- `EternalKidsContext` uitgebreid met `DbSet<PriceRule>` en mapping naar `EternalKids_PriceRule`.
