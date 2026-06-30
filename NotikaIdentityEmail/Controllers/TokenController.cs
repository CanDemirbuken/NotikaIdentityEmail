using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NotikaIdentityEmail.Models.JwtModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NotikaIdentityEmail.Controllers
{
    public class TokenController(IOptions<JwtSettingsModel> jwtSettingsModel) : Controller
    {
        private readonly JwtSettingsModel _jwtSettingsModel = jwtSettingsModel.Value;

        public IActionResult Generate()
        {
            ViewBag.Title = "Token Sayfası";
            ViewBag.Description = "Parametreleri girerek token oluşturma işlemi gerçekleştirilebilir.";
            return View();
        }


        [HttpPost]
        public IActionResult Generate(SimpleUserViewModel simpleUserViewModel)
        {
            var claim = new[]
            {
                new Claim("name", simpleUserViewModel.Name),
                new Claim("surname", simpleUserViewModel.Surname),
                new Claim("city", simpleUserViewModel.City),
                new Claim("username", simpleUserViewModel.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettingsModel.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    issuer: _jwtSettingsModel.Issuer,
                    audience: _jwtSettingsModel.Audience,
                    claims: claim,
                    expires: DateTime.UtcNow.AddMinutes(_jwtSettingsModel.ExpireMinutes),
                    signingCredentials: credentials
                );

            simpleUserViewModel.Token = new JwtSecurityTokenHandler().WriteToken(token);
            return View(simpleUserViewModel);
        }
    }
}