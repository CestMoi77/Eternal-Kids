namespace EternalKids.Domain.Enums;

public enum OrderStatus
{
    Draft = 1,
    PaymentPending = 2,
    Paid = 3,
    DepositPaid = 4,
    Confirmed = 5,
    Completed = 6,
    Cancelled = 7
}
