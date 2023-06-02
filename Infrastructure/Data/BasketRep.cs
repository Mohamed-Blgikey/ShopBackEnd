using AutoMapper;
using Core.DTOS.BasketItems;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Extend;
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
        private readonly IMapper mapper;

        public BasketRep(StoreContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<bool> DeleteBasketAsync(string userId)
        {
            var deletedBasket = await context.BasktItems.Where(a => a.UserId == userId).ToListAsync();
            context.BasktItems.RemoveRange(deletedBasket);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<IReadOnlyList<BasketToReturnDto>> GetBasketAsync(string userId)
        {
            return mapper.Map<IReadOnlyList<BasketToReturnDto>>(await context.BasktItems.Where(a => a.UserId == userId).ToListAsync());
        }

        public async Task<BasketToReturnDto> UpdateBasketAsync(BasketToReturnDto basket, string userId)
        {
            BasktItem basktItem = mapper.Map<BasktItem>(basket);
            if (await context.BasktItems.AnyAsync(a => a.UserId == userId && a.Id == basket.Id))
            {
                if (basktItem.Quantity == 0)
                {
                    context.Remove(basktItem);
                }
                else
                {
                    context.BasktItems.Update(basktItem);

                }
            }
            else
            {
                basktItem.Id = 0;
                await context.BasktItems.AddAsync(basktItem);
            }
            await context.SaveChangesAsync();
            return basket;
        }
    }
}
