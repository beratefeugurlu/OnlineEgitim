using System;
using System.Collections.Generic;

namespace OnlineEgitim.AdminAPI.Models
{
    public class Order
    {
        public int Id { get; set; }

        // Kullanıcı ilişkisi
        public int UserId { get; set; }
        public User User { get; set; }

        // Sipariş tarihi
        public DateTime OrderDate { get; set; } = DateTime.Now;

        // Toplam fiyat (opsiyonel, ödeme için işine yarayabilir)
        public decimal TotalPrice { get; set; }

        // OrderItem ilişkisi
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
