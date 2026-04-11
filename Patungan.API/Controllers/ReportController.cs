using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Patungan.Services.Interfaces;
using Patungan.Shared.Responses;
using Patungan.Shared.Responses.Report;

namespace Patungan.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("cashflow/{userId}/{year}/{month}")]
        public async Task<ActionResult<ApiResponse<CashFlowReportResponse>>> GetCashFlowReport(int userId, int year, int month)
        {
            try
            {
                var result = await _reportService.GetCashFlowReportAsync(userId, year, month);
                return Ok(ApiResponse<CashFlowReportResponse>.Ok("Cash flow report retrieved successfully", result));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<CashFlowReportResponse>.Fail(ex.Message));
            }
        }

        [HttpGet("income-expense-comparison/{userId}")]
        public async Task<ActionResult<ApiResponse<IncomeExpenseComparisonResponse>>> GetIncomeExpenseComparison(
            int userId,
            [FromQuery] int startYear,
            [FromQuery] int startMonth,
            [FromQuery] int endYear,
            [FromQuery] int endMonth)
        {
            try
            {
                // Validate query parameters early to avoid DateOnly/DateTime construction errors
                if (startYear <= 0 || endYear <= 0 || startMonth <= 0 || endMonth <= 0)
                    return BadRequest(ApiResponse<IncomeExpenseComparisonResponse>.Fail("Query parameters startYear, startMonth, endYear and endMonth are required and must be > 0"));
                if (startMonth < 1 || startMonth > 12 || endMonth < 1 || endMonth > 12)
                    return BadRequest(ApiResponse<IncomeExpenseComparisonResponse>.Fail("Month must be between 1 and 12"));

                var result = await _reportService.GetIncomeExpenseComparisonAsync(userId, startYear, startMonth, endYear, endMonth);
                return Ok(ApiResponse<IncomeExpenseComparisonResponse>.Ok("Income expense comparison retrieved successfully", result));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<IncomeExpenseComparisonResponse>.Fail(ex.Message));
            }
        }

        [HttpGet("trend-analysis/{userId}")]
        public async Task<ActionResult<ApiResponse<TrendAnalysisResponse>>> GetTrendAnalysis(int userId, [FromQuery] int months = 6)
        {
            try
            {
                var result = await _reportService.GetTrendAnalysisAsync(userId, months);
                return Ok(ApiResponse<TrendAnalysisResponse>.Ok("Trend analysis retrieved successfully", result));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<TrendAnalysisResponse>.Fail(ex.Message));
            }
        }
    }
}
