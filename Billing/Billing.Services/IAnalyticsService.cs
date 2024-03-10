using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Billing.Services;

public interface IAnalyticsService
{
    Task<IReadOnlyCollection<TaskCostModel>> GetTasksCost(DateTime from, DateTime to);
    Task<DailyReport> GetReport(DateTime today);
    Task<AccountReport> GetAccountReport(Guid getUserId);
    Task<SummaryReport> GetSummary();
}