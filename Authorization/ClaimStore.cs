using System.Collections.Generic;
using System.Security.Claims;

namespace BookInventory.Data
{
    public static class ClaimStore
    {
        public static List<Claim> Claims = new List<Claim>()
        {
            new Claim("Admin", "Admin"),
            new Claim("User", "User"),
            new Claim("Adult", "Adult"),
            new Claim("DateOfBirth", "DateOfBirth")
        };
    }
}