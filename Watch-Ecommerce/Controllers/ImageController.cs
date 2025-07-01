using AutoMapper;
using ECommerce.Core.model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Watch_Ecommerce.DTOS.ImageDTO;
using Watch_Ecommerce.Services;
using Watch_EcommerceBl.Interfaces;

namespace Watch_Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageManagementService imageManagementService;
        private readonly IMapper mapper;
        private readonly IUnitOfWorks UOW;

        public ImageController(IUnitOfWorks UOW, IMapper mapper, IImageManagementService imageManagementService)
        {
            this.UOW = UOW;
            this.mapper = mapper;
            this.imageManagementService = imageManagementService;
        }


        [HttpPost]
        public async Task<IActionResult> AddImgProduct([FromForm] AddImgDto addImgDto)
        {
            if (addImgDto == null || addImgDto.Images==null || !addImgDto.Images.Any()) return BadRequest("Image is required");
            var product= await UOW.productrepo.GetByIdWithImagesAsync(addImgDto.ProductId);
            if (product == null) return NotFound("Product not found");
            var imageUrl = await imageManagementService.SaveImagesAsync(addImgDto.Images, product.Name);
            var image = new List<Image>();
            for(int i = 0; i < imageUrl.Count; i++)
            {
                image.Add(new Image
                {
                    Url = imageUrl[i],
                    isPrimary = (i == 0 && addImgDto.isPrimary),
                    ProductId = addImgDto.ProductId,
                });
            }
          
            await UOW.ImageRepository.AddRangeAsync(image);
            await UOW.CompleteAsync();
            return Ok(new { message = "Image added successfully", images = imageUrl });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var resualt = await imageManagementService.DeleteImageAsynce(id);
            if(!resualt)
                return NotFound("Image not found");

            return Ok(new { message = "Image deleted successfully" });
        }
    }

}
