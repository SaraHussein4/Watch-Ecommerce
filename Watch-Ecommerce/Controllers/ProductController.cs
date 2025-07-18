
using System.IO;
using System.Linq;
using AutoMapper;
using ECommerce.Core.model;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Watch_Ecommerce.DTOs.Product;
using Watch_Ecommerce.DTOS.Product;
using Watch_Ecommerce.Services;
using Watch_EcommerceBl.Interfaces;
using Watch_EcommerceBl.UnitOfWorks;
using Watch_EcommerceDAL.Models;


namespace Watch_Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IUnitOfWorks unitOfWork;
        private readonly IMapper mapper;
        private readonly IImageManagementService _imageManagementService;
        private readonly TikrContext _context;

        public ProductController(IUnitOfWorks unitOfWork, IMapper mapper, IImageManagementService imageManagementService, TikrContext context)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            _imageManagementService = imageManagementService;
            _context = context;
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

        [HttpPost("FilterProduct")]
        public async Task<ActionResult> GetFilteredProducts(ProductFilterDTO productFilterDTO)
        {
            var query = _context.Products.AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(productFilterDTO.SearchTerm))
            {
                query = query.Where(p => p.Name.Contains(productFilterDTO.SearchTerm));
            }

            if (productFilterDTO.CategoryIds != null && productFilterDTO.CategoryIds.Any())
            {
                query = query.Where(p => productFilterDTO.CategoryIds.Contains(p.CategoryId));
            }

            if (productFilterDTO.BrandIds != null && productFilterDTO.BrandIds.Any())
            {
                query = query.Where(p => productFilterDTO.BrandIds.Contains(p.ProductBrandId));
            }

            if (productFilterDTO.Genders != null && productFilterDTO.Genders.Any())
            {
                query = query.Where(p => productFilterDTO.Genders.Contains(p.GenderCategory));
            }

            // Apply sorting
            query = productFilterDTO.SortBy switch
            {
                "priceLowToHigh" => query.OrderBy(p => p.Price),
                "priceHighToLow" => query.OrderByDescending(p => p.Price),
                _ => query.OrderByDescending(p => p.Id) // newest first
            };

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply pagination
            var items = await query
                .Skip((productFilterDTO.Page - 1) * productFilterDTO.PageSize)
                .Take(productFilterDTO.PageSize)
                .ToListAsync();


            var DisplayProductsDTO = mapper.Map<List<DisplayProductDTO>>(items);
            return Ok(new
            {
                items = DisplayProductsDTO,
                totalCount = totalCount
            });
        }

        [HttpGet("best-sellers")]
        public async Task<IActionResult> GetTopBestSellers()
        {
            try
            {
                var topProducts = await _context.Products
                    .Include(p => p.Images.Where(i => i.isPrimary))
                    .OrderByDescending(p => p.Id) // Or any other logic (sales count)
                    .Take(8)
                    .ToListAsync();

                var result = mapper.Map<IEnumerable<DisplayProductDTO>>(topProducts);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching best sellers: {ex.Message}");
            }
        }

        [HttpGet("brand-featured")]
        public async Task<IActionResult> GetOneProductPerBrand()    
        {
            try
            {
                var products = await unitOfWork.productrepo.GetOneProductPerBrandAsync();
                var productDTOs = mapper.Map<IEnumerable<DisplayProductDTO>>(products);
                return Ok(productDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving featured brand products: {ex.Message}");
            }
        }


        [HttpGet("{id:int}")]
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
        [Consumes("multipart/form-data")] // Explicitly require multipart
        public async Task<IActionResult> AddProduct([FromForm] ProductCreateDTO productCreateDTO)
        {
            if (productCreateDTO == null)
                return BadRequest("Product data is null.");

            try
            {
                var categoryExists = await unitOfWork.CategoryRepository.ExistsAsync(productCreateDTO.CategoryId);
                var brandExists = await unitOfWork.ProductBrandRepository.ExistsAsync(productCreateDTO.ProductBrandId);
                if (!categoryExists || !brandExists)
                    return BadRequest("Invalid CategoryId or ProductBrandId.");

                var product = mapper.Map<Product>(productCreateDTO);
                product.Status = "available"; // Default status
                await unitOfWork.productrepo.AddAsync(product);
                await unitOfWork.CompleteAsync();



                //if(productCreateDTO.Colors != null && productCreateDTO.Colors.Any())
                //{
                //    var colors = productCreateDTO.Colors.Split(",").Select(c => int.Parse(c));
                //    foreach(var color in colors)
                //    {
                //        await unitOfWork.ProductColorRepository.AddAsync(new ProductColor
                //        {
                //            ProductId = product.Id,
                //            ColorId = color
                //        });
                //    }
                //}

                //if (productCreateDTO.Sizes != null && productCreateDTO.Sizes.Any())
                //{
                //    var sizes = productCreateDTO.Sizes.Split(",").Select(s => int.Parse(s));
                //    foreach (var size in sizes)
                //    {
                //        await unitOfWork.ProductSizeRepository.AddAsync(new ProductSize
                //        {
                //            ProductId = product.Id,
                //            SizeId = size
                //        });
                //    }
                //}


                if (productCreateDTO.Images != null && productCreateDTO.Images.Any())
                {
                    List<string> ImagePath = await _imageManagementService.AddImageAsync(productCreateDTO.Images, productCreateDTO.Name);
                    List<Image> Images = new List<Image>();
                    for(int i = 0;i < ImagePath.Count(); ++i)
                    {
                        if(i == 0)
                        {
                            Images.Add(new Image
                            {
                                Url = ImagePath[i],
                                isPrimary = true,
                                ProductId = product.Id
                            });
                        }
                        else
                        {
                            Images.Add(new Image
                            {
                                Url = ImagePath[i],
                                isPrimary = false,
                                ProductId = product.Id
                            });
                        }
                    }

                    await unitOfWork.ImageRepository.AddRangeAsync(Images);
                    await unitOfWork.CompleteAsync();
 
                }


                var productReadDTO = mapper.Map<DisplayProductDTO>(product);
                return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, productReadDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating product: {ex.InnerException?.Message ?? ex.Message}");
            }
        }


        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] UpdateProductDTO productDto)
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

                //if (productDto.Images != null)
                //{
                //    existingProduct.Images = mapper.Map<List<Image>>(productDto.Images); // Replace or merge images
                //}
                if (productDto.Images != null && productDto.Images.Any())
                {

                    var paths = await _imageManagementService.AddImageAsync(productDto.Images, productDto.Name);
                    var existingImagesCount = existingProduct.Images?.Count ?? 0;
                    bool hasPrimary = existingProduct.Images?.Any(img => img.isPrimary) ?? false;

                    var newImages = paths.Select((path, index) => new Image
                    {
                        Url = path,
                        isPrimary = !hasPrimary && index == 0,
                        ProductId = id
                    }).ToList();

                    await unitOfWork.ImageRepository.AddRangeAsync(newImages);
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
                return NoContent(); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting product: {ex.Message}");
            }
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedProducts(int page = 1, int pageSize = 10)
        {
            try
            {
                var result = await unitOfWork.productrepo.GetPageAsync(page, pageSize);
                var products = result.Item1;
                var totalCount = result.Item2;

                var mappedProducts = mapper.Map<IEnumerable<DisplayProductDTO>>(products);

                return Ok(new
                {
                    items = mappedProducts,
                    totalCount = totalCount
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching paged products: {ex.Message}");
            }
        }
    }
}


