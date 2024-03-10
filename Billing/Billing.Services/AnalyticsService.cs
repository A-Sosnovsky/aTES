using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Billing.DAL;

namespace Billing.Services;

internal sealed class AnalyticsService : IAnalyticsService
{
    private readonly IRepository _repository;

    public AnalyticsService(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<TaskCostModel>> GetTasksCost(DateTime from, DateTime to)
    {
        await Task.Delay(1);
        return Array.Empty<TaskCostModel>();
    }

    public async Task<DailyReport> GetReport(DateTime today)
    {
        await Task.Delay(1);
        return new DailyReport();
    }

    public Task<AccountReport> GetAccountReport(Guid getUserId)
    {
        throw new NotImplementedException();
    }

    public Task<SummaryReport> GetSummary()
    {
        throw new NotImplementedException();
    }
}