using MalaBars.Application.DTO_s;
using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentDto?> GetByOrderIdAsync(int userId, int orderId);
    }
}
