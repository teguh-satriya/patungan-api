using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Patungan.Services.Interfaces;
using Patungan.Shared.Responses;
using Patungan.Shared.Responses.Budget;

namespace Patungan.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetService _budgetService;

        public BudgetController(IBudgetService budgetService)
        {
            _budgetService = budgetService;
        }

        [HttpGet("overview/{userId}/{year}/{month}")]
        public async Task<ActionResult<ApiResponse<BudgetOverviewResponse>>> GetBudgetOverview(int userId, int year, int month)
        {
            try
            {
                var result = await _budgetService.GetBudgetOverviewAsync(userId, year, month);
                return Ok(ApiResponse<BudgetOverviewResponse>.Ok("Budget overview retrieved successfully", result));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<BudgetOverviewResponse>.Fail(ex.Message));
            }
        }

        [HttpGet("spending-by-type/{userId}/{year}/{month}")]
        public async Task<ActionResult<ApiResponse<List<TransactionTypeBudgetResponse>>>> GetSpendingByType(int userId, int year, int month)
        {
            try
            {
                var result = await _budgetService.GetSpendingByTypeAsync(userId, year, month);
                return Ok(ApiResponse<List<TransactionTypeBudgetResponse>>.Ok("Spending by type retrieved successfully", result));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<List<TransactionTypeBudgetResponse>>.Fail(ex.Message));
            }
        }

        [HttpGet("projected-balance/{userId}/{year}/{month}")]
        public async Task<ActionResult<ApiResponse<decimal>>> GetProjectedEndingBalance(int userId, int year, int month)
        {
            try
            {
                var result = await _budgetService.GetProjectedEndingBalanceAsync(userId, year, month);
                return Ok(ApiResponse<decimal>.Ok("Projected ending balance retrieved successfully", result));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<decimal>.Fail(ex.Message));
            }
        }

        [HttpGet("carryover-history/{userId}")]
        public async Task<ActionResult<ApiResponse<CarryoverHistoryResponse>>> GetCarryoverHistory(
            int userId,
            [FromQuery] int fromYear,
            [FromQuery] int fromMonth,
            [FromQuery] int toYear,
            [FromQuery] int toMonth)
        {
            try
            {
                var result = await _budgetService.GetCarryoverHistoryAsync(userId, fromYear, fromMonth, toYear, toMonth);
                return Ok(ApiResponse<CarryoverHistoryResponse>.Ok("Carryover history retrieved successfully", result));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<CarryoverHistoryResponse>.Fail(ex.Message));
            }
        }
    }
}
