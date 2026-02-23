# Eternal Kids – Fase 6 (Checkout & Order Management)

## Opgeleverd

1. **Checkout pagina** (`/Checkout`)
   - Klantgegevens + eventgegevens
   - Betaalkeuze: `Volledig` of `50% aanbetaling`
   - Overzicht van subtotal/totaal/deposit

2. **Order creatie**
   - `IOrderService` + `OrderService`
   - Shopping cart wordt omgezet naar `Order` + `OrderItems`
   - Startstatus op `Draft`, daarna `PaymentPending` tijdens betaalstart

3. **Payment mock**
   - `IPaymentService` + `PaymentServiceMock`
   - Simuleert providerflow (Mollie/Stripe-achtig)
   - Na succesvolle simulatie:
     - `Paid` bij volledig betalen
     - `DepositPaid` bij 50% aanbetaling

## Nieuwe onderdelen

- DTOs: `CheckoutDtos` (`CreateOrderFromCartRequest/Result`, `StartPaymentRequest/Result`)
- Contracts: `IOrderService`, `IPaymentService`
- Services: `OrderService`, `PaymentServiceMock`
- Enum uitbreiding: `OrderStatus.DepositPaid`, nieuwe `PaymentMode`
