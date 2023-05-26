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

        
    }
}
