using MalaBars.Application.Interfaces;
using MalaBars.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Infrastructure.Services
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ApplicationDbContext _applicationDbContext;

        public UnitOfWork(ApplicationDbContext applicationDbContext) // grouping multiple DB operation int one unit of work
            // such as create order, create orderItem, clear cart etc...
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action) // T could be anything like OrderDto, int, bool, Product etc..
        {
            await using var transaction = await _applicationDbContext.Database.BeginTransactionAsync();
            //Use this transaction, and when we're finished, clean it up properly."
            try
            {
                var result = await action();

                await transaction.CommitAsync();

                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

        }
    }
}
