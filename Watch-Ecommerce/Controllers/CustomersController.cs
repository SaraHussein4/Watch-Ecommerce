using System.Security.Claims;
using AutoMapper;
using ECommerce.Core.model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Watch_Ecommerce.DTOS.Address;
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
        private readonly UserManager<User> _userManager;

        public CustomersController(IUnitOfWorks unitOfWorks, IMapper mapper, UserManager<User> userManager)
        {
            _unitOfWorks = unitOfWorks;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserReadDTO>> GetCustomers()
        {
            var customers = _unitOfWorks.UserRepository.GetCustomers();
            var customersReadDTO = _mapper.Map<IEnumerable<UserReadDTO>>(customers);
            return Ok(customersReadDTO);
        }

        [Authorize]
        [HttpGet("Profile")]
        public async Task<ActionResult<UserProfileReadDTO>> GetCustomerProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
             return Unauthorized();

            var customer = await _unitOfWorks.UserRepository.GetByIdWithAddressesAsync(userId);
            if (customer == null)
             return NotFound();

            var customerProfileReadDTO = _mapper.Map<UserProfileReadDTO>(customer);
            return Ok(customerProfileReadDTO);
        }

        [Authorize]
        [HttpPut("Profile")]
        public async Task<IActionResult> UpdateCustomerProfile([FromBody] UserProfileUpdateDTO userProfileUpdateDTO)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var customer = await _unitOfWorks.UserRepository.GetByIdWithAddressesAsync(userId);
            if (customer == null)
                return NotFound();

            // Update scalar fields
            customer.Name = userProfileUpdateDTO.Name;
            customer.PhoneNumber = userProfileUpdateDTO.PhoneNumber;

            // Handle address updates
            var incomingAddresses = userProfileUpdateDTO.Addresses ?? new List<AddressDTO>();

            // 1. Remove deleted addresses
            var addressesToRemove = customer.Addresses
                .Where(addr => !incomingAddresses.Any(dto => dto.Id == addr.Id))
                .ToList();

            foreach (var address in addressesToRemove)
            {
                customer.Addresses.Remove(address);
            }

            // 2. Update existing + Add new
            foreach (var dto in incomingAddresses)
            {
                if (dto.Id == 0)
                {
                    var newAddress = _mapper.Map<Address>(dto);
                    newAddress.UserId = customer.Id;
                    customer.Addresses.Add(newAddress);
                }
                else
                {
                    var existing = customer.Addresses.FirstOrDefault(a => a.Id == dto.Id);
                    if (existing != null)
                    {
                        _mapper.Map(dto, existing);
                    }
                    else
                    {
                        return BadRequest($"Address ID {dto.Id} does not belong to the current user.");
                    }
                }
            }

            // 3. Enforce only one default address
            var defaultCount = customer.Addresses.Count(a => a.IsDefault);

            if (defaultCount > 1)
            {
                return BadRequest("Only one address can be marked as default.");
            }

            // Optional: if none marked default, auto-assign the first one
            if (defaultCount == 0 && customer.Addresses.Any())
            {
                customer.Addresses.First().IsDefault = true;
            }

            await _unitOfWorks.CompleteAsync();
            return NoContent();
        }

        [Authorize]
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] UserChangePawwordDTO changePasswordDTO)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            var result = await _userManager.ChangePasswordAsync(user, changePasswordDTO.CurrentPassword, changePasswordDTO.NewPassword);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToArray();
                return BadRequest(new { statusCode = 400, errors });
            }

            return NoContent();
        }




    }
}
