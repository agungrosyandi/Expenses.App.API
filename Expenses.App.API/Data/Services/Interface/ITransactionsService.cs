using Expenses.App.API.Dtos.Transaction;
using Expenses.App.API.Models;

namespace Expenses.App.API.Data.Services.Interface;

public interface ITransactionsService
{
    // add ------------------------
    Task<Transaction> AddAsync(PostTransactionDto transaction, int userId);

    // delete ------------------------
    Task<bool> DeleteAsync(int id, int userId);

    // get all ------------------------
    Task<List<Transaction>> GetAllAsync(int userId);

    // get by id ------------------------
    Task<Transaction?> GetByIdAsync(int id, int userId);

    // update ------------------------
    Task<Transaction?> UpdateAsync(int id, PutTransactionDto transaction, int userId);
}
