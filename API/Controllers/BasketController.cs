using Core.DTOS.BasketItems;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRep basketRep;

        public BasketController(IBasketRep basketRep)
        {
            this.basketRep = basketRep;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<BasketToReturnDto>>> Get(string userId)
        {
            return Ok(await basketRep.GetBasketAsync(userId));
        }

        [HttpPost]
        public async Task<ActionResult<BasketToReturnDto>> UpdateBasket(BasketToReturnDto basketToReturnDto)
        {
            return Ok(await basketRep.UpdateBasketAsync(basketToReturnDto, basketToReturnDto.UserId));
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string userId)
        {
            return Ok(await basketRep.DeleteBasketAsync(userId));
        }


    }
}
