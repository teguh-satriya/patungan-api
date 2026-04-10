using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Patungan.Services.Interfaces;
using Patungan.Shared.Requests.TransactionType;
using Patungan.Shared.Responses;
using Patungan.Shared.Responses.TransactionType;

namespace Patungan.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionTypeController : ControllerBase
    {
        private readonly ITransactionTypeService _transactionTypeService;

        public TransactionTypeController(ITransactionTypeService transactionTypeService)
        {
            _transactionTypeService = transactionTypeService;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<TransactionTypeResponse>>> AddTransactionType([FromBody] CreateTransactionTypeRequest request)
        {
            try
            {
                var result = await _transactionTypeService.AddTransactionTypeAsync(request);
                return Ok(ApiResponse<TransactionTypeResponse>.Ok("Transaction type created", result));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<TransactionTypeResponse>.Fail(ex.Message));
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ApiResponse<IReadOnlyList<TransactionTypeResponse>>>> GetByUser(int userId)
        {
            try
            {
                var result = await _transactionTypeService.GetTransactionTypesByUserAsync(userId);
                return Ok(ApiResponse<IReadOnlyList<TransactionTypeResponse>>.Ok("Transaction types retrieved", result));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<IReadOnlyList<TransactionTypeResponse>>.Fail(ex.Message));
            }
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse<TransactionTypeResponse>>> Update([FromBody] UpdateTransactionTypeRequest request)
        {
            try
            {
                var result = await _transactionTypeService.UpdateTransactionTypeAsync(request);
                return Ok(ApiResponse<TransactionTypeResponse>.Ok("Transaction type updated", result));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<TransactionTypeResponse>.Fail(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        {
            try
            {
                await _transactionTypeService.DeleteTransactionTypeAsync(id);
                return Ok(ApiResponse<object>.Ok("Transaction type deleted", null));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Fail(ex.Message));
            }
        }
    }
}
