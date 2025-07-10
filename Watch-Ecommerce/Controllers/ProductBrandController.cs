using AutoMapper;
using ECommerce.Core.model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Watch_Ecommerce.DTOS.Category;
using Watch_Ecommerce.DTOS.ProductBrand;
using Watch_EcommerceBl.Interfaces;

namespace Watch_Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductBrandController : ControllerBase
    {
        private readonly IUnitOfWorks _unitOfWorks;
        private readonly IMapper _mapper;

        public ProductBrandController(IUnitOfWorks unitOfWorks, IMapper mapper)
        {
            _unitOfWorks = unitOfWorks;
            _mapper = mapper;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductBrandReadDTO>>> GetBrands()
        {
            var Brands = await _unitOfWorks.ProductBrandRepository.GetAllAsync();
            var BrandsReadDTO = _mapper.Map<IEnumerable<ProductBrandReadDTO>>(Brands);
            return Ok(BrandsReadDTO);
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductBrandReadDTO>> GetBrand(int id)
        {
            var brand = await _unitOfWorks.ProductBrandRepository.GetByIdAsync(id);

            if (brand == null)
            {
                return NotFound();
            }

            var brandReadDTO = _mapper.Map<ProductBrandReadDTO>(brand);
            return Ok(brandReadDTO);
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBrand(int id, ProductBrandUpdateDTO productBrandUpdateDTO)
        {
            if (id != productBrandUpdateDTO.Id)
            {
                return BadRequest();
            }

            var brand = await _unitOfWorks.ProductBrandRepository.GetByIdAsync(id);
            if (brand == null)
            {
                return BadRequest();
            }
            _mapper.Map(productBrandUpdateDTO, brand);
            await _unitOfWorks.ProductBrandRepository.UpdateAsync(brand);
            await _unitOfWorks.CompleteAsync();
            return NoContent();
        }

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductBrandReadDTO>> PostBrand(ProductBrandCreateDTO productBrandCreateDTO)
        {
            var exists = await _unitOfWorks.ProductBrandRepository
                      .AnyAsync(b => b.Name.ToLower() == productBrandCreateDTO.Name.ToLower());

            if (exists)
            {
                return BadRequest("Brand already exists.");
            }

            var brand = _mapper.Map<ProductBrand>(productBrandCreateDTO);
            await _unitOfWorks.ProductBrandRepository.AddAsync(brand);
            await _unitOfWorks.CompleteAsync();

            var productBrandReadDTO = _mapper.Map<ProductBrandReadDTO>(brand);

            return CreatedAtAction(nameof(GetBrand), new { id = brand.Id }, productBrandReadDTO);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            var brand = await _unitOfWorks.ProductBrandRepository.GetByIdAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            await _unitOfWorks.ProductBrandRepository.DeleteAsync(id);
            await _unitOfWorks.CompleteAsync();

            return NoContent();
        }

        private async Task<bool> BrandExists(int id)
        {
            return await _unitOfWorks.ProductBrandRepository.ExistsAsync(id);
        }
    }
}
