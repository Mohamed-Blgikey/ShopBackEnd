using Core.DTOS.BasketItems;
using Infrastructure.Extend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTOS
{
    public class CustomerBasket
    {
        public string Id { get; set; }
        public List<BasketToReturnDto> Items { get; set; } = new List<BasketToReturnDto>();

        public int? DeliveryMethodId { get; set; }

        public string ClientSecret { get; set; }

        public string PaymentIntentId { get; set; }

        public decimal ShippingPrice { get; set; }
    }
}
