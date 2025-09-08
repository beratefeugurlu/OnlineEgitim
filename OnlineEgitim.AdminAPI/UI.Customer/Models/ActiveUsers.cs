
using System.Collections.Generic;

namespace UI.Customer.Models
{
 
    public static class ActiveUsers
    {
       
        public static HashSet<string> Users { get; } = new HashSet<string>();

       
        public static void Add(string email)
        {
            if (!string.IsNullOrEmpty(email))
                Users.Add(email);
        }

      
        public static void Remove(string email)
        {
            if (!string.IsNullOrEmpty(email) && Users.Contains(email))
                Users.Remove(email);
        }
    }
}






























