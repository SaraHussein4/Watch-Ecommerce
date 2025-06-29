using AutoMapper;
using ECommerce.Core.model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Watch_Ecommerce.DTOs.Fav;
using Watch_EcommerceBl.Interfaces;

namespace Watch_Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FavController : ControllerBase
    {
        IMapper mapper;
        IUnitOfWorks UOW;
        public FavController(IMapper mapper, IUnitOfWorks UOW)
        {
            this.mapper = mapper;
            this.UOW = UOW;
        }
        //[HttpPost]
        //public async Task<IActionResult> AddProductToFavorite(FavDto favDto)
        //{
        //    if (favDto == null) return BadRequest();
        //    if (!ModelState.IsValid) return BadRequest(ModelState);

        //        var userclaims = User.FindFirst(ClaimTypes.NameIdentifier);
        //        if (userclaims == null)
        //            return Unauthorized("User not authenticated");
        //        string userId = userclaims.Value;
        //        var prod = await UOW.productrepo.GetProductByIdAsync(favDto.ProductId);
        //      if(prod == null) return NotFound("Product not found");
        //      int productId = prod.Id;
        //       var myFav= await UOW.FavoriteRepo.AddToFav(userId, productId);
        //        if (myFav == null) return Conflict("Item already exists in Favourite.");
        //    await UOW.CompleteAsync();
        //        var myFavDto=mapper.Map<FavDto>(myFav);
        //        return Ok(myFavDto);
        //}
        [HttpPost]
        public async Task<IActionResult> AddProductToFavorite(FavDto favDto)
        {
            if (favDto == null) return BadRequest();
            if (ModelState.IsValid)
            {
                var userclaims = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userclaims == null)
                    return Unauthorized("User not authenticated");
                string userId = userclaims.Value;
                var prod = await UOW.productrepo.GetProductByIdAsync(favDto.ProductId);
                if (prod == null) return NotFound("Product not found");
                int productId = prod.Id;
                Favourite myFav = await UOW.FavoriteRepo.AddToFav(userId, productId);
                if (myFav == null) return BadRequest("Failed to add item to Favourite.");
                UOW.CompleteAsync();
                var myFavDto = mapper.Map<FavDto>(myFav);
                return Ok(myFavDto);
            }
            return BadRequest(ModelState);
        }
        [HttpDelete("{prodid}")]
        public async Task<IActionResult> RemoveFromFavourite(int prodid)
        {

            var userclaims = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userclaims == null)
                return Unauthorized("User not authenticated");
            string userId = userclaims.Value;
            var prod = await UOW.productrepo.GetProductByIdAsync(prodid);
            if (prod == null) return NotFound("Product not found");
            int productId = prod.Id;
            var favRemoved = await UOW.FavoriteRepo.RemoveFromFav(userId, productId);
            if (!favRemoved) return NotFound("Item not found in favorites");
            await UOW.CompleteAsync();
            return Ok("Item removed successfully");
        }
    }
}
