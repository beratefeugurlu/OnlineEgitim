using System.Collections.Generic;

namespace UI.Customer.Models
{
    public static class ActiveUsers
    {
        // Sistem aktif kullanıcıları burada saklanır
        public static HashSet<string> Users { get; } = new HashSet<string>();

        // Kullanıcı giriş yaptığında ekleme
        public static void Add(string email)
        {
            if (!string.IsNullOrEmpty(email))
                Users.Add(email);
        }

        // Kullanıcı çıkış yaptığında silme
        public static void Remove(string email)
        {
            if (!string.IsNullOrEmpty(email) && Users.Contains(email))
                Users.Remove(email);
        }
    }
}
