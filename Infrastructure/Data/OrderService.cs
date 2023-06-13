using AutoMapper;
using Core.DTOS;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specification.OrderWithItemsAndOrderingSpec;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class OrderService : IOrderService
    {
        private readonly IUniteOdWork uniteOdWork;
        private readonly IBasketRep basketRep;
        private readonly StoreContext context;
        private readonly IMapper mapper;

        public OrderService(IUniteOdWork uniteOdWork, IBasketRep basketRep,StoreContext context ,IMapper mapper)
        {
            this.uniteOdWork = uniteOdWork;
            this.basketRep = basketRep;
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodid, string userId, OrderAddress shippingAddress)
        {
            var basket = await basketRep.GetBasketAsync(userId);

            var items = new List<OrderItem>();
            foreach (var item in basket)
            {

                var itemOrdered = new ProductItemOrdered()
                {
                    PictureUrl = item.PhotoUrl,
                    ProductItemId = item.Id,
                    ProductName = item.ProductName,
                };
                var orderItem = new OrderItem(itemOrdered, item.Price, item.Quantity);
                items.Add(orderItem);
            }

            var deliveryMethod = await uniteOdWork.repository<DeliveryMethod>().GetByIdAsync(deliveryMethodid);
            var subtotal = items.Sum(a => a.Price * a.Quantity);

            var order = new Order(items,buyerEmail,shippingAddress,deliveryMethod,subtotal,null);
            uniteOdWork.repository<Order>().Add(order);
            var result = await uniteOdWork.Complete();
            await basketRep.DeleteBasketAsync(userId);
            if (result<=0)
                return null;
            return order;   
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await uniteOdWork.repository<DeliveryMethod>().GetAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var spec = new OrderWithItemsAndOrderingSpec(id, buyerEmail);
            var x = await context.Orders.Where(a => a.BuyerEmail == buyerEmail && a.Id == id).Include(a => a.DeliveryMethod).Include(a => a.ShipToAddress).Include(a => a.OrderItems).ThenInclude(a => a.ItemOrdered).FirstOrDefaultAsync();
            return x;

        }

        public async Task<List<Order>> GetOrdersForUserAsync(string byuyerEmail)
        {
            var spec = new OrderWithItemsAndOrderingSpec(byuyerEmail);
            var x = await context.Orders.Where(a => a.BuyerEmail == byuyerEmail).Include(a => a.DeliveryMethod).Include(a => a.ShipToAddress).Include(a => a.OrderItems).ThenInclude(a => a.ItemOrdered).ToListAsync();
            return x;
        }
    }
}
