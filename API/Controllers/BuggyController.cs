using API.Error;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    public class BuggyController : BaseAPIController
    {
        private readonly StoreContext context;

        public BuggyController(StoreContext context)
        {
            this.context = context;
        }

        [HttpGet("GetNotFound")]
        public IActionResult GetNotFound() {
            var t = context.Products.Find(100);
            if (t == null)
            {
                return NotFound(new ApiResponse(404,null));
            }
            return Ok(t);
        }

        [HttpGet("GetServerError")]
        public IActionResult GetServerError()
        {
            var t = context.Products.Find(100);
            var ts = t.Price.ToString();
            return Ok(t);
        }

        [HttpGet("BadRequest")]
        public IActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400,null));
        }
        [HttpGet("BadRequest/I{id}")]
        public IActionResult GetBadRequest(int id)
        {
            return Ok();
        }

    }
}
