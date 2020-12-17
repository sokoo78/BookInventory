using BookInventory.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookInventory.Authentication
{
    public static class TokenGenerator
    {
        public static string GenerateJWTToken(ApplicationUser user, IList<Claim> claims)
        {            
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.UserName));
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SD.SecretKey));
            var sinature = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            var token = new JwtSecurityToken(
            issuer: SD.Issuer,
            audience: SD.Audience,
            claims: claims,
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: sinature
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
