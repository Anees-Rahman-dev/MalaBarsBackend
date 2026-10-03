using MalaBars.Application.DTO_s;
using MalaBars.Application.Interfaces;
using MalaBars.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;

        public PaymentService(
       IPaymentRepository paymentRepository,
       IOrderRepository orderRepository)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
        }

        public async Task<PaymentDto?> GetByOrderIdAsync(int userId, int orderId )
        {
            var order = await _orderRepository.GetByIdAsync(orderId);

            if (order == null || order.UserId != userId) 
                // there is user with that id then only true;
                return null;

            var payment = await _paymentRepository.GetByOrderIdAsync(orderId);

            if (payment == null)
                return null;

            return new PaymentDto
            {
                PaymentId = payment.PaymentId,
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                PaymentMethod = payment.PaymentMethod,
                PaymentStatus = payment.PaymentStatus,
                PaymentDate = payment.PaymentDate
            };
        }

        public async Task<PaymentDto?> GetAdminPaymentByOrderIdAsync(int orderId)
        {
            var payment = await _paymentRepository.GetByOrderIdAsync(orderId);

            if (payment == null)
                return null;

            return new PaymentDto
            {
                PaymentId = payment.PaymentId,
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                PaymentMethod = payment.PaymentMethod,
                PaymentStatus = payment.PaymentStatus,
                PaymentDate = payment.PaymentDate
            };
        }
    }
}
