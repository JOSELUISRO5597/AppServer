using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AppServer.Helpers;
using AppServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AppServer.Controllers
{
    public class TokenController 
    {
        public static String CreateToken(IConfiguration  configuration,[FromBody]User login)
        {
            var response = String.Empty;

                var tokenString = BuildToken(configuration, login);
                response = tokenString;
            

            return response;
        }

        public static string BuildToken(IConfiguration configuration, User user)
        {
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
              configuration["Jwt:Issuer"],
              claims:claims,
              expires: DateTime.Now.AddDays(7),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}