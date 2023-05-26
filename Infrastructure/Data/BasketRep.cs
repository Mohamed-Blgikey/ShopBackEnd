using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class BasketRep : IBasketRep
    {
        private readonly StoreContext context;

        public BasketRep( StoreContext context)
        {
            this.context = context;
        }
       

    }
}
