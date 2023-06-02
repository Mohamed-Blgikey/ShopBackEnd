using Core.DTOS.BasketItems;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IBasketRep
    {
        Task<IReadOnlyList<BasketToReturnDto>> GetBasketAsync(string userId);
        Task<BasketToReturnDto> UpdateBasketAsync(BasketToReturnDto basket, string userId);
        Task<bool> DeleteBasketAsync(string userId);
    }
}
