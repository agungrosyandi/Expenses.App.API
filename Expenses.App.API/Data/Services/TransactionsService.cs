using Expenses.App.API.Data.Services.Interface;
using Expenses.App.API.Dtos.auth;
using Expenses.App.API.Dtos.Transaction;
using Expenses.App.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Expenses.App.API.Data.Services;

public class TransactionsService(AppDbContext appDbContext) : ITransactionsService
{
    // ---------------- Add ------------------------

    public async Task<Transaction> AddAsync(PostTransactionDto transaction, int userId)
    {
        var newTransaction = new Transaction()
        {
            Amount = transaction.Amount,
            Type = transaction.Type,
            Category = transaction.Category,
            CreateAt = DateTime.UtcNow,
            UpdateAt = DateTime.UtcNow,
            UserId = userId
        };

        await appDbContext.Transactions.AddAsync(newTransaction);
        await appDbContext.SaveChangesAsync();

        // Optionally load User after insert

        await appDbContext.Entry(newTransaction).Reference(t => t.User).LoadAsync();

        return newTransaction;
    }

    // ---------------- delete ------------------------

    public async Task<bool> DeleteAsync(int id, int userId)
    {
        var transactionDb = await appDbContext.Transactions.FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);

        if (transactionDb is null)
        {
            return false;
        }

        appDbContext.Transactions.Remove(transactionDb);

        await appDbContext.SaveChangesAsync();

        return true;
    }

    // ---------------- get all ------------------------

    public async Task<List<Transaction>> GetAllAsync(int userId)
    {
        return await appDbContext.Transactions
                        .Where(t => t.UserId == userId)
                        .Include(t => t.User)
                        .ToListAsync();
    }

    // ---------------- get by id ------------------------

    public async Task<Transaction?> GetByIdAsync(int id, int userId)
    {
        return await appDbContext.Transactions
                        .Include(t => t.User)
                        .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
    }

    // ---------------- update ------------------------

    public async Task<Transaction?> UpdateAsync(int id, PutTransactionDto transaction, int userId)
    {
        var transactionDb = await appDbContext.Transactions.FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);

        if (transactionDb is null)
        {
            return null;
        }

        transactionDb.Type = transaction.Type;
        transactionDb.Amount = transaction.Amount;
        transactionDb.Category = transaction.Category;
        transactionDb.UpdateAt = DateTime.UtcNow;

        await appDbContext.SaveChangesAsync();

        // Optionally load User after update
        await appDbContext.Entry(transactionDb).Reference(t => t.User).LoadAsync();

        return transactionDb;
    }
}
