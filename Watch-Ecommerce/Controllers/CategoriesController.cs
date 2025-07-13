using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerce.Core.model;
using Watch_Ecommerce.DTOS.Category;
using Watch_EcommerceBl.Interfaces;
using AutoMapper;

namespace Watch_Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWorks _unitOfWorks;
        private readonly IMapper _mapper;

        public CategoriesController(IUnitOfWorks unitOfWorks, IMapper mapper)
        {
            _unitOfWorks = unitOfWorks;
            _mapper = mapper;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryReadDTO>>> GetCategories()
        {
            var categories = await _unitOfWorks.CategoryRepository.GetAllAsync();
            var categoriesReadDTO = _mapper.Map<IEnumerable<CategoryReadDTO>>(categories);
            return Ok(categoriesReadDTO);
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryReadDTO>> GetCategory(int id)
        {
            var category = await _unitOfWorks.CategoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            var categoryReadDTO = _mapper.Map<CategoryReadDTO>(category);
            return Ok(categoryReadDTO);
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, CategoryUpdateDTO categoryUpdateDTO)
        {
            if (id != categoryUpdateDTO.Id)
            {
                return BadRequest();
            }

            var category = await _unitOfWorks.CategoryRepository.GetByIdAsync(id);
            if(category == null)
            {
                return BadRequest();
            }
            _mapper.Map(categoryUpdateDTO, category);
            await _unitOfWorks.CategoryRepository.UpdateAsync(category);
            await _unitOfWorks.CompleteAsync();
            return NoContent();
        }

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CategoryReadDTO>> PostCategory(CategoryCreateDTO categoryCreateDTO)
        {
            var exists = await _unitOfWorks.CategoryRepository
                    .AnyAsync(c => c.Name.ToLower() == categoryCreateDTO.Name.ToLower());

            if (exists)
            {
                return BadRequest("Category already exists.");
            }

            var category = _mapper.Map<Category>(categoryCreateDTO);
            await _unitOfWorks.CategoryRepository.AddAsync(category);
            await _unitOfWorks.CompleteAsync();

            var categoryReadDTO = _mapper.Map<CategoryReadDTO>(category);

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, categoryReadDTO);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _unitOfWorks.CategoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            await _unitOfWorks.CategoryRepository.DeleteAsync(id);
            await _unitOfWorks.CompleteAsync();

            return NoContent();
        }

        private async Task<bool> CategoryExists(int id)
        {
            return await _unitOfWorks.CategoryRepository.ExistsAsync(id);
        }
    }
}
