using API.Error;
using AutoMapper;
using Core.DTOS;
using Core.DTOS.Auth;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Infrastructure.Extend;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var email = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var address = new OrderAddress(orderDto.ShipToAddress.FirstName, orderDto.ShipToAddress.LastName, orderDto.ShipToAddress.Street, orderDto.ShipToAddress.City, orderDto.ShipToAddress.State, orderDto.ShipToAddress.ZipCode);


            var order = await _orderService.CreateOrderAsync(email, orderDto.DeliveryMethodId, orderDto.userId,address);

            if (order == null) return BadRequest(new ApiResponse(400, "Problem creating order"));

            return Ok(order);
        }

        [HttpGet]
        public async Task<ActionResult<List<Order>>> GetOrdersForUser()
        {
            var email = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var orders = await _orderService.GetOrdersForUserAsync(email);
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderByIdForUser(int id)
        {
            var email = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var order = await _orderService.GetOrderByIdAsync(id, email);
            if (order == null) return NotFound(new ApiResponse(404,null));
            return order;
        }

        [HttpGet("deliveryMethod")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            return Ok(await _orderService.GetDeliveryMethodsAsync());
        }
    }
}
