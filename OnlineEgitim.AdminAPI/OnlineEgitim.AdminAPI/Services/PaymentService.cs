using OnlineEgitim.AdminAPI.Models;
using OnlineEgitim.AdminAPI.Repositories;

namespace OnlineEgitim.AdminAPI.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<OrderItem> _orderItemRepository;
        private readonly IRepository<Course> _courseRepository;

        public PaymentService(
            IRepository<Order> orderRepository,
            IRepository<OrderItem> orderItemRepository,
            IRepository<Course> courseRepository)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _courseRepository = courseRepository;
        }

        public async Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request)
        {
            // fake odeme doğrulama
            if (request.CardNumber != "1111222233334444")
            {
                return new PaymentResponse
                {
                    Success = false,
                    Message = "Ödeme reddedildi! Kart numarası geçersiz."
                };
            }

            
            var order = new Order
            {
                UserId = request.UserId,
                OrderDate = DateTime.Now
            };

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();

            //  Kurs fiyatlarını DB’den çekerek siparişe ekleme_____
            foreach (var courseId in request.CourseIds)
            {
                var course = await _courseRepository.GetByIdAsync(courseId);

                if (course != null)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        CourseId = course.Id,
                        Quantity = 1,
                        Price = course.Price 
                    };

                    await _orderItemRepository.AddAsync(orderItem);
                }
            }

            await _orderItemRepository.SaveChangesAsync();

            return new PaymentResponse
            {
                Success = true,
                Message = "Ödeme başarılı ✅",
                OrderId = order.Id
            };
        }
    }
}
