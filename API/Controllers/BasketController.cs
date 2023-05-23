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

        [HttpGet("GetBasket")]
        public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
        {
            return Ok(await basketRep.GetBasketAsync(id));
        }

        [HttpPost("UpdateBasket")]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasket basket)
        {
            var updated = await basketRep.UpdateBasketAsync(basket);
            return Ok(updated);
        }
        [HttpDelete("DeleteBasket")]
        public async Task<ActionResult<bool>> DeleteBasket(string basketId)
        {
            var updated = await basketRep.DeleteBasketAsync(basketId);
            return Ok(updated);
        }
    }
}
