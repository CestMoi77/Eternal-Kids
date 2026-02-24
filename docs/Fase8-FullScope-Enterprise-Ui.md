# Eternal Kids – Full Scope uitbreiding (3.1 t/m 3.6)

## Geleverd in deze update

- Enterprise UI refactor met kleurenpalet `#F9F7F2`, `#C5A367`, `#1A1A1A` in `site.css`.
- Sticky navigatie + uitgebreide menu-items + gouden `BOEK NU` knop.
- Nieuwe publieke pagina's:
  - `Home` (hero, diensten, reviews)
  - `Diensten`
  - `Pakketten` (cards + vergelijkingsmatrix)
  - `Reserveren` (multi-step, eventtype-afhankelijke velden)
  - `Contact` (formulier)
- AI Planner verbeterd met `Direct Boeken` knop die naar checkout pusht.
- Admin uitgebreid met `Content` beheerpagina.
- Datamodel uitgebreid met:
  - `PackageDefinition`
  - `SiteContentBlock`
- EF migratiebestand toegevoegd: `20260223_AddPackageAndContentTables`.

## Let op

In deze container ontbreekt de .NET SDK, dus runtime build-/migratie-executie kon niet lokaal worden uitgevoerd.
