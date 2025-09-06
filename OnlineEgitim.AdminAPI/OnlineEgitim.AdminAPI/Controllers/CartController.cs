using Microsoft.AspNetCore.Mvc;
using OnlineEgitim.AdminAPI.Models;
using OnlineEgitim.AdminAPI.Repositories;

namespace OnlineEgitim.AdminAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly IRepository<Cart> _cartRepository;

        public CartController(IRepository<Cart> cartRepository)
        {
            _cartRepository = cartRepository;
        }

        // GET: api/Cart
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var carts = await _cartRepository.GetAllAsync();
            return Ok(carts);
        }

        // GET: api/Cart/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cart = await _cartRepository.GetByIdAsync(id);
            if (cart == null) return NotFound();
            return Ok(cart);
        }

        // POST: api/Cart
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Cart cart)
        {
            await _cartRepository.AddAsync(cart);
            await _cartRepository.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = cart.Id }, cart);
        }

        // PUT: api/Cart/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Cart updatedCart)
        {
            var cart = await _cartRepository.GetByIdAsync(id);
            if (cart == null) return NotFound();

            cart.UserId = updatedCart.UserId;
            cart.CourseId = updatedCart.CourseId;
            cart.Quantity = updatedCart.Quantity;

            _cartRepository.Update(cart);
            await _cartRepository.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Cart/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cart = await _cartRepository.GetByIdAsync(id);
            if (cart == null) return NotFound();

            _cartRepository.Delete(cart);
            await _cartRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}

