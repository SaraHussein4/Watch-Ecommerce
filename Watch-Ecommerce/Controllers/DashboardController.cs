using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Watch_EcommerceBl.Interfaces;
using Watch_EcommerceDAL.Models;
using Microsoft.EntityFrameworkCore;
using ECommerce.Core.model;

namespace Watch_Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IUnitOfWorks _unitOfWorks;
        private readonly TikrContext _context;

        public DashboardController(IUnitOfWorks unitOfWorks, TikrContext context)
        {
            _unitOfWorks = unitOfWorks;
            _context = context;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var totalProducts = (await _unitOfWorks.productrepo.GetAllAsync()).Count();
            var ordersList = await _context.Orders.ToListAsync();
            var totalOrders = ordersList.Count;
            var totalCustomers = _unitOfWorks.UserRepository.GetCustomers().Count();
            var revenue = ordersList.Sum(o => o.Amount);

            var lastMonth = DateTime.Now.AddMonths(-1);
            var revenueLastMonth = ordersList.Where(o => o.Date >= lastMonth).Sum(o => o.Amount);
            var growth= revenueLastMonth == 0 ? "0%" : $"{Math.Round(((revenue - revenueLastMonth) / revenueLastMonth) * 100, 2)}";


            return Ok(new
            {
                totalProducts,
                totalOrders,
                totalCustomers,
                revenue,
                revenueGrowth = growth
            });
        }
      
    }
}