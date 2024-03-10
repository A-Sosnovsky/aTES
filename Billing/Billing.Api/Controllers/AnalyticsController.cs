using System;
using System.Threading.Tasks;
using Billing.DAL;
using Billing.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Billing.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _analyticsService;

    public AnalyticsController(IAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    /// <summary>
    /// показывать самую дорогую задачу за день, неделю или месяц
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    [HttpGet("tasks-cost")]
    public async Task<IActionResult> GetTasksCost(DateTime from, DateTime to)
    {
        var cost = await _analyticsService.GetTasksCost(from, to);
        return Ok(cost);
    }
    
    /// <summary>
    /// сколько заработал топ-менеджмент за сегодня и сколько попугов ушло в минус
    /// </summary>
    /// <param name="today"></param>
    /// <returns></returns>
    [HttpGet("daily-report")]
    public async Task<IActionResult> GetReport(DateTime today)
    {
        var report = await _analyticsService.GetReport(today);
        return Ok(report);
    }
    
    /// <summary>
    /// информация о счете для текущего пользователя
    /// </summary>
    /// <returns></returns>
    [HttpGet("account-info")]
    public async Task<IActionResult> GetAccountInfo()
    {
        var result = await _analyticsService.GetAccountReport(this.GetUserId());
        return Ok(result);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var report = await _analyticsService.GetSummary();
        return Ok(report);
    }
}