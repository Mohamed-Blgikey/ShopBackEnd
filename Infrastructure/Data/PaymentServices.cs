using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Infrastructure.DTOS;
using Infrastructure.Interfaces;
using Infrastructure.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace Infrastructure.Data
{
    public class PaymentServices : IPaymentServices
    {
        private readonly IUniteOdWork _unitOfWork;
        private readonly IBasketRep _basketRepo;
        private readonly IConfiguration _config;
        private readonly StoreContext context;

        public PaymentServices(IUniteOdWork unitOfWork, IBasketRep basketRepo, IConfiguration config,StoreContext context)
        {
            _unitOfWork = unitOfWork;
            _basketRepo = basketRepo;
            _config = config;
            this.context = context;
        }
        public async Task<Order> CreateOrUpdatePaymentIntent(int orderId)
        {
            StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];

            Order order = await context.Orders.Include(a=>a.DeliveryMethod).Include(a=>a.OrderItems).FirstOrDefaultAsync(a=>a.Id == orderId);
            if (order == null)
            {
                return null;
            }
            else
            {
                var shippingPrice = order.DeliveryMethod.Price;


                var service = new PaymentIntentService();

                PaymentIntent intent;

                if (string.IsNullOrEmpty(order.PaymentIntentId))
                {
                    var options = new PaymentIntentCreateOptions
                    {
                        Amount = (long)order.OrderItems.Sum(i => i.Quantity * (i.Price * 100)) +
                                 (long)shippingPrice * 100,
                        Currency = "usd",
                        PaymentMethodTypes = new List<string> { "card" }
                    };

                    intent = await service.CreateAsync(options);
                    order.PaymentIntentId = intent.Id;
                    order.ClientSecret = intent.ClientSecret;
                }
                else
                {
                    var options = new PaymentIntentUpdateOptions
                    {
                        Amount = (long)order.OrderItems.Sum(i => i.Quantity * (i.Price * 100)) +
                                 (long)shippingPrice * 100
                    };

                    await service.UpdateAsync(order.PaymentIntentId, options);
                }

            }

           
            await context.SaveChangesAsync();
            return  order;
        }

        public async Task<Order> UpdateOrderPaymentFailed(string paymentIntentId)
        {
            var order = await context.Orders.Include(a=>a.OrderItems).ThenInclude(a=>a.ItemOrdered).Include(a=>a.DeliveryMethod).Include(a=>a.ShipToAddress).FirstOrDefaultAsync(a=>a.PaymentIntentId == paymentIntentId);

            if (order == null) return null;
            order.Status = OrderStatus.PaymentFailed;
            await _unitOfWork.Complete();

            return order;
        }

        public async Task<Order> UpdateOrderPaymentSucceded(string paymentIntentId)
        {
            var order = await context.Orders.Include(a => a.OrderItems).ThenInclude(a => a.ItemOrdered).Include(a => a.DeliveryMethod).Include(a => a.ShipToAddress).FirstOrDefaultAsync(a => a.PaymentIntentId == paymentIntentId);

            if (order == null) return null;
            order.Status = OrderStatus.PaymentReceived;
            await _unitOfWork.Complete();

            return order;
        }
    }
}
