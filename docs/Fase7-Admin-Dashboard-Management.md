# Eternal Kids – Fase 7 (Admin Dashboard & Management)

## Opgeleverd

- **/Admin/Orders**: beveiligd orderoverzicht met statusfilter (`Paid`, `DepositPaid`) en doorklik naar detailpagina.
- **/Admin/Orders/{id}**: orderdetails inclusief `OptionsJson` per orderitem.
- **/Admin/Pricing**: beheerinterface voor `PriceRule` records (toevoegen/verwijderen).
- **/Admin/AiLogs**: overzicht van `EternalKids_AI_Request_Log` voor AI-analyses.
- **Admin layout**: zakelijke UI met zijbalknavigatie en Eternal Kids branding.

## Security

- Alle admin pagina's zijn voorzien van `[Authorize]`.

## Branding

- Admin UI gebruikt een zakelijke dark-sidebar stijl, maar blijft binnen Eternal Kids kleur-/brandingkader.
