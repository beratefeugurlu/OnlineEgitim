using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using UI.Customer.Extensions;
using UI.Customer.Models;

namespace UI.Customer.Controllers
{
    public class CartController : Controller
    {
        private const string CartSessionKey = "Cart";

        private List<UI.Customer.Models.Course> GetCart()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<UI.Customer.Models.Course>>(CartSessionKey);
            if (cart == null)
            {
                cart = new List<UI.Customer.Models.Course>();
                HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);
            }
            return cart;
        }

        private void SaveCart(List<UI.Customer.Models.Course> cart)
        {
            HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);
        }

        //  Sepeti görüntüleme
        public IActionResult Index()
        {
            var cart = GetCart(); 
            return View(cart);   
        }

        //  Sepete ekle.  Ajax uyumlu hale getirildi
        [HttpPost]
        public IActionResult Add(int id, string title, decimal price)
        {
            var cart = GetCart();

            if (!cart.Any(c => c.Id == id))
            {
                cart.Add(new UI.Customer.Models.Course
                {
                    Id = id,
                    Title = title,
                    Price = price
                });

                SaveCart(cart);
            }

            return Ok(new { success = true, message = $"{title} sepete eklendi!" });
        }

        //  Sepetten kaldırma islemleri.
        [HttpPost]
        public IActionResult Remove(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.Id == id);
            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }

            return Ok(new { success = true, message = "Ürün sepetten kaldırıldı!" });
        }

      
        [HttpPost]
        public IActionResult Clear()
        {
            SaveCart(new List<UI.Customer.Models.Course>());
            return Ok(new { success = true, message = "Sepet temizlendi!" });
        }
    }
}
