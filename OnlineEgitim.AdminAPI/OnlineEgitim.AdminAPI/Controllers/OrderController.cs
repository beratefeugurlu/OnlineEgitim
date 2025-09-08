using Microsoft.AspNetCore.Mvc;
using OnlineEgitim.AdminAPI.Models;
using OnlineEgitim.AdminAPI.Repositories;

namespace OnlineEgitim.AdminAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<OrderItem> _orderItemRepository;
        private readonly IRepository<Course> _courseRepository;
        private readonly IRepository<User> _userRepository;

        public OrderController(
            IRepository<Order> orderRepository,
            IRepository<OrderItem> orderItemRepository,
            IRepository<Course> courseRepository,
            IRepository<User> userRepository)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _courseRepository = courseRepository;
            _userRepository = userRepository;
        }

        
        [HttpGet("User/{userId}")]
        public async Task<IActionResult> GetOrdersByUser(int userId)
        {
            var orders = await _orderRepository.GetAllAsync();
            var userOrders = orders.Where(o => o.UserId == userId).ToList();

            if (!userOrders.Any())
                return Ok(new List<object>());

            var orderItems = await _orderItemRepository.GetAllAsync();
            var userItems = orderItems.Where(i => userOrders.Any(o => o.Id == i.OrderId)).ToList();

            var purchasedCourses = new List<object>();

            var user = await _userRepository.GetByIdAsync(userId);

            foreach (var item in userItems)
            {
                var course = await _courseRepository.GetByIdAsync(item.CourseId);
                if (course != null)
                {
                    purchasedCourses.Add(new
                    {
                        UserName = user?.Name,
                        UserEmail = user?.Email,
                        CourseTitle = course.Title,
                        CoursePrice = course.Price,
                        PurchaseDate = DateTime.Now
                    });
                }
            }

            return Ok(purchasedCourses);
        }
    }
}
