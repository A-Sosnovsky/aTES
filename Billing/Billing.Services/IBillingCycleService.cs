using System.Threading.Tasks;

namespace Billing.Services;

public interface IBillingCycleService
{
    public Task<long> GetCurrentBillingCycleId();
    Task CalculateCurrenBillingCycle();
}