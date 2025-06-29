
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
                var products = await unitOfWork.productrepo.GetAllWithPrimaryImageAsync(); // Include images
                var productDTOs = mapper.Map<IEnumerable<DisplayProductDTO>>(products);
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
                var product = await unitOfWork.productrepo.GetByIdWithImagesAsync(id); // ← Include Images
                if (product == null)
                    return NotFound($"Product with ID {id} not found.");

                var productDTO = mapper.Map<DisplayProductDTO>(product);
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
                var categoryExists = await unitOfWork.CategoryRepository.ExistsAsync(productDto.CategoryId);
                var brandExists = await unitOfWork.ProductBrandRepository.ExistsAsync(productDto.ProductBrandId);
                if (!categoryExists || !brandExists)
                    return BadRequest("Invalid CategoryId or ProductBrandId.");

                var product = mapper.Map<Product>(productDto);

                if (productDto.Images != null && productDto.Images.Any())
                {
                    product.Images = mapper.Map<List<Image>>(productDto.Images);
                }

                await unitOfWork.productrepo.AddAsync(product);
                await unitOfWork.CompleteAsync();

                var productReadDTO = mapper.Map<DisplayProductDTO>(product);
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
                var existingProduct = await unitOfWork.productrepo.GetByIdWithImagesAsync(id);
                if (existingProduct == null)
                    return NotFound($"Product with ID {id} not found.");

                var categoryExists = await unitOfWork.CategoryRepository.ExistsAsync(productDto.CategoryId);
                var brandExists = await unitOfWork.ProductBrandRepository.ExistsAsync(productDto.ProductBrandId);
                if (!categoryExists || !brandExists)
                    return BadRequest("Invalid CategoryId or ProductBrandId.");

                mapper.Map(productDto, existingProduct);

                if (productDto.Images != null)
                {
                    existingProduct.Images = mapper.Map<List<Image>>(productDto.Images); // Replace or merge images
                }

                await unitOfWork.productrepo.UpdateAsync(existingProduct);
                await unitOfWork.CompleteAsync();

                var productReadDTO = mapper.Map<DisplayProductDTO>(existingProduct);
                return Ok(productReadDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating product: {ex.InnerException?.Message ?? ex.Message}");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id) { 
            try
            {
                var product = await unitOfWork.productrepo.GetByIdAsync(id);
                if (product == null)
                    return NotFound($"Product with ID {id} not found.");
                await unitOfWork.productrepo.DeleteAsync(id);
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


