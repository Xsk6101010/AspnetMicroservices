using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using EventBus.Messages.Events;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly DiscountGrpcService _discountGrpcService;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository repository, DiscountGrpcService discountGrpcService, IMapper mapper)
        {
            _repository = repository;
            _discountGrpcService = discountGrpcService;
            _mapper = mapper;
        }

        [HttpGet("{name}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string name)
        {
            var basket = await _repository.Get(name);
            return Ok(basket ?? new ShoppingCart(name));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> Update([FromBody] ShoppingCart basket)
        {
            // TODO:
            // Communicate with Discount.Grpc and Calculate latest prices of product into shopping cart
            // consume Discount Grpc

            foreach (var item in basket.Items)
            {
                var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }

            return Ok(await _repository.Update(basket));
        }

        [HttpDelete("{name}", Name = "DeleteBasket")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            await _repository.Delete(userName);
            return Ok();
        }

        [Route("Action")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            // get existing basket with total price.
            var basket = await _repository.Get(basketCheckout.UserName);
            if (basket == null) return BadRequest();

            // send checkout event to rabbitmq.
            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);

            // _eventBus.PublishBasketCheckout.

            // remove the basket.
            await _repository.Delete(basket.UserName);
            return Accepted();
        }
    }
}
