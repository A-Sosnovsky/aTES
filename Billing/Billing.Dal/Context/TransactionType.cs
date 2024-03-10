namespace Billing.DAL.Context;

public enum TransactionType
{
    // Зачисление
    Enrollment = 1,
    
    // Списание
    Withdrawal = 2,
    
    // Выплата
    Payment = 3
}