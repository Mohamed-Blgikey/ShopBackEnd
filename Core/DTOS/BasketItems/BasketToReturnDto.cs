using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOS.BasketItems
{
    public class BasketToReturnDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string PhotoUrl { get; set; }
        public string Brand { get; set; }
        public string Type { get; set; }
        public string UserId { get; set; }
    }
}
