
﻿using AutoMapper;
using ECommerce.Core.model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Watch_Ecommerce.DTOs.Product;
using Watch_Ecommerce.DTOS.Product;
using Watch_EcommerceBl.Interfaces;
using Watch_EcommerceBl.UnitOfWorks;


namespace Watch_Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IUnitOfWorks unitOfWork;
        private readonly IMapper mapper;

        public ProductController(IUnitOfWorks unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await unitOfWork.ProductRepository.GetAllAsync();

                // Map to DTOs
                var productDTOs = mapper.Map<IEnumerable<ProductReadDTO>>(products);
                return Ok(productDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving products: {ex.Message}");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await unitOfWork.ProductRepository.GetByIdAsync(id);
                if (product == null)
                    return NotFound($"Product with ID {id} not found.");

                var productDTO = mapper.Map<ProductReadDTO>(product); 
                return Ok(productDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving product: {ex.Message}");
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] AddProductDTO productDto)
        {
            if (productDto == null)
                return BadRequest("Product data is null.");

            try
            {
                // Lookup CategoryId
                var categories = await unitOfWork.CategoryRepository.GetAllAsync();
                var category = categories.FirstOrDefault(c => c.Name.ToLower() == productDto.CategoryName.ToLower());

                // Lookup BrandId
                var brands = await unitOfWork.ProductBrandRepository.GetAllAsync();
                var brand = brands.FirstOrDefault(b => b.Name.ToLower() == productDto.ProductBrandName.ToLower());

                if (category == null || brand == null)
                {
                    return BadRequest("Invalid Category or Brand name.");
                }

                // Map DTO to Entity
                var product = mapper.Map<Product>(productDto);
                product.CategoryId = category.Id;
                product.ProductBrandId = brand.Id;

                await unitOfWork.ProductRepository.AddAsync(product);
                await unitOfWork.CompleteAsync();

                var productReadDTO = mapper.Map<ProductReadDTO>(product);
                return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, productReadDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating product: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDTO productDto)
        {
            if (productDto == null || productDto.Id != id)
                return BadRequest("Invalid data.");

            try
            {
                var existingProduct = await unitOfWork.ProductRepository.GetByIdAsync(id);
                if (existingProduct == null)
                    return NotFound($"Product with ID {id} not found.");

                // Lookup Category and Brand by name
                var category = (await unitOfWork.CategoryRepository.GetAllAsync())
                                    .FirstOrDefault(c => c.Name.ToLower() == productDto.CategoryName.ToLower());
                var brand = (await unitOfWork.ProductBrandRepository.GetAllAsync())
                                    .FirstOrDefault(b => b.Name.ToLower() == productDto.ProductBrandName.ToLower());

                if (category == null || brand == null)
                    return BadRequest("Invalid Category or Brand name.");

                // Map updated values
                mapper.Map(productDto, existingProduct);
                existingProduct.CategoryId = category.Id;
                existingProduct.ProductBrandId = brand.Id;

                await unitOfWork.ProductRepository.UpdateAsync(existingProduct);
                await unitOfWork.CompleteAsync();

                var productReadDTO = mapper.Map<ProductReadDTO>(existingProduct);
                return Ok(productReadDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating product: {ex.Message}");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id) { 
            try
            {
                var product = await unitOfWork.ProductRepository.GetByIdAsync(id);
                if (product == null)
                    return NotFound($"Product with ID {id} not found.");
                await unitOfWork.ProductRepository.DeleteAsync(id);
                await unitOfWork.CompleteAsync();
                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting product: {ex.Message}");
            }
        }
    }
}


