using System;
using System.Threading;
using System.Threading.Tasks;
using Billing.Services;
using CronScheduler.Extensions.Scheduler;
using Microsoft.Extensions.DependencyInjection;

namespace Billing.Api.BackgroundJobs;

public class BillingCycleCalculationJob : IScheduledJob
{
    private readonly IServiceProvider _provider;

    public BillingCycleCalculationJob(IServiceProvider provider)
    {
        _provider = provider;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var scope = _provider.CreateScope();
        var billingCycleService = _provider.GetRequiredService<IBillingCycleService>();

        await billingCycleService.CalculateCurrenBillingCycle();
    }

    public string Name => nameof(BillingCycleCalculationJob);
}