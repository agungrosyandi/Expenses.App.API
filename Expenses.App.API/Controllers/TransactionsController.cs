using Expenses.App.API.Data;
using Expenses.App.API.Data.Services.Interface;
using Expenses.App.API.Dtos.Transaction;
using Expenses.App.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Security.Claims;

namespace Expenses.App.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[EnableCors("AllowAll")]
[Authorize]
public class TransactionsController(ITransactionsService transactionsService) : ControllerBase
{
    private int? GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return int.TryParse(claim, out var id) ? id : null;
    }

    // get all ------------------------

    [HttpGet("All")]
    public async Task<IActionResult> GetAll()
    {
        var userId = GetUserId();

        if (userId is null)
        {
            return Unauthorized();
        }

        var allTransactions = await transactionsService.GetAllAsync(userId.Value);

        return Ok(allTransactions);
    }

    // get details by id ------------------------

    [HttpGet("Details/{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var userId = GetUserId();

        if (userId is null)
        {
            return Unauthorized();
        }

        var transactionDb = await transactionsService.GetByIdAsync(id, userId.Value);

        if (transactionDb is null)
        {
            return NotFound(new { message = "Data tidak ditemukan" });
        }

        return Ok(transactionDb);
    }

    // create ------------------------

    [HttpPost("Create")]
    public async Task<IActionResult> CreateTransaction([FromBody] PostTransactionDto payload)
    {
        var userId = GetUserId();

        if (userId is null)
        {
            return Unauthorized();
        }

        await transactionsService.AddAsync(payload, userId.Value);

        return Ok(new { message = "Data berhasil dibuat" });
    }

    // update ------------------------

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> UpdateTransaction(int id, [FromBody] PutTransactionDto payload)
    {
        var userId = GetUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        var updateTransactionDb = await transactionsService.UpdateAsync(id, payload, userId.Value);

        if (updateTransactionDb is null)
        {
            return NotFound();
        }

        return Ok(updateTransactionDb);
    }

    // delete ------------------------

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteTransaction(int id)
    {
        var userId = GetUserId();

        if (userId is null)
        {
            return Unauthorized();
        }

        var success = await transactionsService.DeleteAsync(id, userId.Value);

        if (!success)
        {
            return NotFound();
        }

        return Ok(new { message = "Data berhasil dihapus" });
    }
}
