using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Patungan.Services.Interfaces;
using Patungan.Shared.Requests.Transaction;
using Patungan.Shared.Responses;
using Patungan.Shared.Responses.Transaction;

namespace Patungan.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<TransactionResponse>>> CreateTransaction([FromBody] CreateTransactionRequest request)
        {
            try
            {
                var result = await _transactionService.CreateTransactionAsync(request);
                return Ok(ApiResponse<TransactionResponse>.Ok("Transaction created successfully", result));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<TransactionResponse>.Fail(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<TransactionResponse>>> UpdateTransaction(int id, [FromBody] UpdateTransactionRequest request)
        {
            try
            {
                var result = await _transactionService.UpdateTransactionAsync(id, request);
                return Ok(ApiResponse<TransactionResponse>.Ok("Transaction updated successfully", result));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<TransactionResponse>.Fail(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteTransaction(int id)
        {
            try
            {
                await _transactionService.DeleteTransactionAsync(id);
                return Ok(ApiResponse<object>.Ok("Transaction deleted successfully", null));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Fail(ex.Message));
            }
        }

        [HttpGet("monthly/{userId}/{year}/{month}")]
        public async Task<ActionResult<ApiResponse<List<TransactionResponse>>>> GetMonthlyTransactions(int userId, int year, int month)
        {
            try
            {
                var result = await _transactionService.GetMonthlyTransactionsAsync(userId, year, month);
                return Ok(ApiResponse<List<TransactionResponse>>.Ok("Transactions retrieved successfully", result));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<List<TransactionResponse>>.Fail(ex.Message));
            }
        }

        [HttpGet("by-type/{userId}/{transactionTypeId}")]
        public async Task<ActionResult<ApiResponse<List<TransactionResponse>>>> GetTransactionsByType(
            int userId, 
            int transactionTypeId, 
            [FromQuery] DateOnly from, 
            [FromQuery] DateOnly to)
        {
            try
            {
                var result = await _transactionService.GetTransactionsByTypeAsync(userId, transactionTypeId, from, to);
                return Ok(ApiResponse<List<TransactionResponse>>.Ok("Transactions retrieved successfully", result));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<List<TransactionResponse>>.Fail(ex.Message));
            }
        }

        [HttpGet("income/{userId}/{year}/{month}")]
        public async Task<ActionResult<ApiResponse<decimal>>> GetMonthlyIncome(int userId, int year, int month)
        {
            try
            {
                var result = await _transactionService.GetMonthlyIncomeAsync(userId, year, month);
                return Ok(ApiResponse<decimal>.Ok("Monthly income retrieved successfully", result));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<decimal>.Fail(ex.Message));
            }
        }

        [HttpGet("expense/{userId}/{year}/{month}")]
        public async Task<ActionResult<ApiResponse<decimal>>> GetMonthlyExpense(int userId, int year, int month)
        {
            try
            {
                var result = await _transactionService.GetMonthlyExpenseAsync(userId, year, month);
                return Ok(ApiResponse<decimal>.Ok("Monthly expense retrieved successfully", result));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<decimal>.Fail(ex.Message));
            }
        }
    }
}
