using AutoMapper;
using ECommerce.Core.model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Watch_Ecommerce.DTOs.Product;
using Watch_Ecommerce.DTOS.Category;
using Watch_Ecommerce.DTOS.Fav;
using Watch_EcommerceBl.Interfaces;
using Watch_EcommerceBl.UnitOfWorks;

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

        [HttpPost]
        public async Task<IActionResult> AddProductToFavorite(int ProductId)
        {
            if (ModelState.IsValid)
            {
                var userclaims = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userclaims == null)
                    return Unauthorized("User not authenticated");

                string userId = userclaims.Value;

                var prod = await UOW.productrepo.GetProductByIdAsync(ProductId);
                if (prod == null) return NotFound("Product not found");

                int productId = prod.Id;

                Favourite myFav = await UOW.FavoriteRepo.AddToFav(userId, productId);

                if (myFav == null)
                    return BadRequest("Product is already in your Favourite list.");

                await UOW.CompleteAsync();

                var favouriteReadDTO = mapper.Map<FavReadDTO>(myFav);

                return CreatedAtAction(nameof(GetFavourite), new { ProductId = productId, UserId = userId }, favouriteReadDTO);
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


        [HttpGet]
        public async Task<ActionResult<IEnumerable<FavReadDTO>>> GetFavourites()
        {
            var userclaims = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userclaims == null)
                return Unauthorized("User not authenticated");
            string userId = userclaims.Value;
            var favourites = await UOW.FavoriteRepo.GetAllForUser(userId);
            var favouritesReadDTO = mapper.Map<IEnumerable<DisplayProductDTO>>(favourites);
            return Ok(favouritesReadDTO);
        }

        [HttpGet("{ProductId}/{UserId}")]
        public async Task<ActionResult<FavReadDTO>> GetFavourite(int ProductId, string UserId)
        {
            var favourite = await UOW.FavoriteRepo.GetByIdAsync(ProductId, UserId);
            if (favourite == null) return NotFound("");
            var favReadDTO= mapper.Map<FavReadDTO>(favourite);
            return Ok(favReadDTO);
        }


        [HttpGet("count")]
        public async Task<ActionResult<int>> GetFavouritesCount()
        {
            var userclaims = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userclaims == null)
                return Unauthorized("User not authenticated");
            string userId = userclaims.Value;
            int count = await UOW.FavoriteRepo.GetCountAsync(userId);
            return Ok(count);
        }

    }
}
