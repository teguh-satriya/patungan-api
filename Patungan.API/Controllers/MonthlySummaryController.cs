using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Patungan.Services.Interfaces;
using Patungan.Shared.Responses;
using Patungan.Shared.Responses.MonthlySummary;

namespace Patungan.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MonthlySummaryController : ControllerBase
    {
        private readonly IMonthlySummaryService _monthlySummaryService;

        public MonthlySummaryController(IMonthlySummaryService monthlySummaryService)
        {
            _monthlySummaryService = monthlySummaryService;
        }

        [HttpGet("{userId}/{year}/{month}")]
        public async Task<ActionResult<ApiResponse<MonthlySummaryResponse>>> GetMonthlySummary(int userId, int year, int month)
        {
            try
            {
                var result = await _monthlySummaryService.GetOrCreateMonthlySummaryAsync(userId, year, month);
                return Ok(ApiResponse<MonthlySummaryResponse>.Ok("Monthly summary retrieved successfully", result));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<MonthlySummaryResponse>.Fail(ex.Message));
            }
        }

        [HttpGet("yearly/{userId}/{year}")]
        public async Task<ActionResult<ApiResponse<List<MonthlySummaryResponse>>>> GetYearlySummaries(int userId, int year)
        {
            try
            {
                var result = await _monthlySummaryService.GetYearlySummariesAsync(userId, year);
                return Ok(ApiResponse<List<MonthlySummaryResponse>>.Ok("Yearly summaries retrieved successfully", result));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<List<MonthlySummaryResponse>>.Fail(ex.Message));
            }
        }

        [HttpPost("recalculate/{userId}/{year}/{month}")]
        public async Task<ActionResult<ApiResponse<object>>> RecalculateMonthlySummary(int userId, int year, int month)
        {
            try
            {
                await _monthlySummaryService.RecalculateMonthlySummaryAsync(userId, year, month);
                return Ok(ApiResponse<object>.Ok("Monthly summary recalculated successfully", null));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Fail(ex.Message));
            }
        }

        [HttpPost("carryover/{userId}/{year}/{month}")]
        public async Task<ActionResult<ApiResponse<object>>> CarryOverBudget(int userId, int year, int month)
        {
            try
            {
                await _monthlySummaryService.CarryOverBudgetToNextMonthAsync(userId, year, month);
                return Ok(ApiResponse<object>.Ok("Budget carried over to next month successfully", null));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Fail(ex.Message));
            }
        }
    }
}
