using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action);
        //What this does

        // It gives the Application layer a simple instruction:

//      "Run these operations as one transaction."
    }
}
