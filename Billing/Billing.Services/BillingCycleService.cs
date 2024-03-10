using System;
using System.Linq;
using System.Threading.Tasks;
using Billing.DAL;
using Billing.DAL.Context;
using Billing.Services.Events;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Billing.Services;

internal sealed class BillingCycleService : IBillingCycleService
{
    private readonly IRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventsService _eventsService;
    
    public BillingCycleService(IRepository repository, IUnitOfWork unitOfWork, IEventsService eventsService)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _eventsService = eventsService;
    }

    public async Task<long> GetCurrentBillingCycleId()
    {
        var now = DateTime.UtcNow;
        var billingCycle = _repository
            .Query<BillingCycle>()
            .FirstOrDefault(bc => bc.StartDate <= now && bc.EndDate >= now && bc.Status == BillingCycleStatus.Active);
        
        if (billingCycle == null)
        {
            billingCycle = new BillingCycle
            {
                StartDate = now.Date,
                EndDate = now.AddDays(1).AddTicks(-1),
                PublicId = Guid.NewGuid(),
                Status = BillingCycleStatus.Active
            };
            await _repository.InsertAsync(billingCycle);
            await _unitOfWork.Commit();
        }
        
        return billingCycle.Id;
    }

    public async Task CalculateCurrenBillingCycle()
    {
        var now = DateTime.UtcNow;
        
        var billingCycle = _repository
            .Query<BillingCycle>()
            .FirstOrDefault(bc => bc.StartDate == now.Date.AddDays(-1) && bc.Status == BillingCycleStatus.Active);

        if (billingCycle == null)
        {
            throw new Exception("Previous billing cycle not found");
        }
        
        var transactions = await _repository
            .Query<Transaction>()
            .Where(t => t.BillingCycleId == billingCycle.Id)
            .ToArrayAsync();

        foreach (var g in transactions.GroupBy(t => t.AccountId))
        {
            var account = await _repository.Query<Account>().FirstOrDefaultAsync(a => a.Id == g.Key);
            if (account == null)
            {
                throw new Exception("Account not found");
            }

            var credit = g.Select(t => t.Credit).Sum();
            var debit = g.Select(t => t.Debit).Sum();
            
            var totalAmount = debit - credit;
            // если начислено больше, чем списано
            // то выплачиваем попугу деньги и отправляем событие
            if (totalAmount > 0)
            {
                account.Balance = 0;

                var transaction = new Transaction
                {
                    Account = account,
                    BillingCycle = billingCycle,
                    Type = TransactionType.Payment,
                    Credit = 0,
                    Debit = totalAmount,
                    Description = "Payment for billing cycle",
                    PublicId = Guid.NewGuid(),
                    Timespamp = DateTime.UtcNow,
                };
                
                await _eventsService.TransactionCompleted(transaction.PublicId, account.PublicId, totalAmount, TransactionType.Payment);
            }
        }

        billingCycle.Status = BillingCycleStatus.Closed;
        _repository.Update(billingCycle);
        
        await _unitOfWork.Commit();
    }
}