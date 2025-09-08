using System;
using System.Collections.Generic;

namespace OnlineEgitim.AdminAPI.Models
{
    public class Order
    {
        public int Id { get; set; }

      
        public int UserId { get; set; }
        public User User { get; set; }

       
        public DateTime OrderDate { get; set; } = DateTime.Now;

        // Toplam fiyat 
        public decimal TotalPrice { get; set; }

        
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
