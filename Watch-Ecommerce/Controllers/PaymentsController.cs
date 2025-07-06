using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Watch_Ecommerce.DTOS.BasketDto;
using Watch_Ecommerce.Services;
using Watch_EcommerceDAL.Models;

namespace Watch_Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentServices paymentServices;
        private readonly IMapper mapper;

        public PaymentsController(IPaymentServices paymentServices,IMapper mapper)
        {
            this.paymentServices = paymentServices;
            this.mapper = mapper;
        }
        //create or update endpoint
        [HttpPost]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePayment(string basketId)
        {
            var customerBasket=await paymentServices.CreateOrUpdatePayment(basketId);
            if (customerBasket == null)  return BadRequest();
            var mappedBasket=mapper.Map<CustomerBasket,CustomerBasketDto>(customerBasket);
            return Ok(mappedBasket);
        }
    }
}
