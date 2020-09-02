using System;
using System.Text;
using CampaignSaber.Models;
using CampaignSaber.Services;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using CampaignSaber.Authorization;
using CampaignSaber.Models.Discord;
using CampaignSaber.Models.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace CampaignSaber.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IJWTSettings _jwtSettings;
        private readonly DiscordService _discordService;
        private readonly CampaignSaberContext _campaignSaberContext;

        public AuthorizationController(IJWTSettings jwtSettings, DiscordService discordService, CampaignSaberContext campaignSaberContext)
        {
            _jwtSettings = jwtSettings;
            _discordService = discordService;
            _campaignSaberContext = campaignSaberContext;
        }

        [HttpGet]
        public IActionResult Authenticate()
        {
            return Redirect($"https://discordapp.com/api/oauth2/authorize?response_type=code&client_id={_discordService.ID}&scope=identify&redirect_uri={_discordService.RedirectURL}");
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback([FromQuery(Name = "code")] string code)
        {
            string token = await _discordService.GetAccessToken(code);

            // Code was invalid? Let's stop.
            if (token == null)
                return NotFound();

            DiscordUser profile = await _discordService.GetProfile(token);

            // Try to find the user in the database.
            User user = await _campaignSaberContext.Users.FirstOrDefaultAsync(u => u.Id == profile.Id);
            if (user == null)
            {
                // Create the user if there is no user.
                user = new User
                {
                    Id = profile.Id,
                    Profile = profile
                };

                await _campaignSaberContext.Users.AddAsync(user);
                await _campaignSaberContext.SaveChangesAsync();
            }
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var secToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Issuer,
                claims,
                expires: DateTime.UtcNow.AddHours(288f),
                signingCredentials: credentials);

            token = new JwtSecurityTokenHandler().WriteToken(secToken);
            return Ok(new { token });
        }

        [CSAuthorize]
        [HttpGet("@me")]
        public IActionResult GetSelf()
        {
            if (!(HttpContext.Items["User"] is User user))
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}