using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Watch_Ecommerce.DTOS.User;
using Watch_EcommerceBl.Interfaces;

namespace Watch_Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IUnitOfWorks _unitOfWorks;
        private readonly IMapper _mapper;

        public CustomersController(IUnitOfWorks unitOfWorks, IMapper mapper)
        {
            _unitOfWorks = unitOfWorks;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserReadDTO>> GetCustomers()
        {
            var customers = _unitOfWorks.UserRepository.GetCustomers();
            var customersReadDTO = _mapper.Map<IEnumerable<UserReadDTO>>(customers);
            return Ok(customersReadDTO);
        }
    }
}
