using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specification.OrderWithItemsAndOrderingSpec;
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

        public OrderService(IUniteOdWork uniteOdWork, IBasketRep basketRep)
        {
            this.uniteOdWork = uniteOdWork;
            this.basketRep = basketRep;
        }
        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodid, string userId, OrderAddress shippingAddress)
        {
            var basket = await basketRep.GetBasketAsync(userId);

            var items = new List<OrderItem>();
            foreach (var item in basket)
            {
                var productItem = await uniteOdWork.repository<Product>().GetByIdAsync(item.Id);
                var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
                var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
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

            return await uniteOdWork.repository<Order>().GetWithSpecAsync(spec);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string byuyerEmail)
        {
            var spec = new OrderWithItemsAndOrderingSpec(byuyerEmail);

            return await uniteOdWork.repository<Order>().ListWithSpecAsync(spec);
        }
    }
}
