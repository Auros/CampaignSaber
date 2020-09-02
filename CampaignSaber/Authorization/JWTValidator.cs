using System;
using System.Linq;
using System.Text;
using CampaignSaber.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using CampaignSaber.Models.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace CampaignSaber.Authorization
{
    public class JWTValidator
    {
        private readonly RequestDelegate _next;
        private readonly IJWTSettings _jwtSettings;

        public JWTValidator(RequestDelegate next, IJWTSettings jwtSettings)
        {
            _next = next;
            _jwtSettings = jwtSettings;
        }

        public async Task Invoke(HttpContext context, CampaignSaberContext campaignSaberContext)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
            if (token != null)
                await AttachUserToContext(context, campaignSaberContext, token);
            await _next(context);

        }

        private async Task AttachUserToContext(HttpContext context, CampaignSaberContext campaignSaberContext, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidAudience = _jwtSettings.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "sub").Value;
                context.Items["User"] = await campaignSaberContext.Users.FirstAsync(u => u.Id == userId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}