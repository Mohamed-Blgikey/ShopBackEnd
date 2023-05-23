using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class CustomerBasket
    {
        public CustomerBasket()
        {
            
        }
        public CustomerBasket(string Id)
        {
            this.Id = Id;
        }
        public string Id { get; set; }
        public List<BasktItem> BasktItems { get; set; } = new List<BasktItem>();
    }
}
