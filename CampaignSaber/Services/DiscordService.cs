using CampaignSaber.Models.Discord;
using CampaignSaber.Models.Settings;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CampaignSaber.Services
{
    public class DiscordService
    {
        private const string _discordUserURLString = "https://discordapp.com/api/users/@me";
        private const string _discordAuthURLString = "https://discordapp.com/api/oauth2/token";

        private readonly HttpClient _client;

        public string ID { get; private set; }
        public string Token { get; private set; }
        public string Secret { get; private set; }
        public string RedirectURL { get; private set; }


        public DiscordService(HttpClient client, IDiscordSettings settings)
        {
            _client = client;

            ID = settings.ID;
            Token = settings.Token;
            Secret = settings.Secret;
            RedirectURL = settings.RedirectURL;
        }

        public async Task<string> GetAccessToken(string code)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "client_id", ID },
                { "grant_type", "authorization_code" },
                { "code", code },
                { "redirect_url", RedirectURL }
            };
            FormUrlEncodedContent content = new FormUrlEncodedContent(parameters);
            HttpResponseMessage response = await _client.PostAsync(_discordAuthURLString, content).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                Stream responseStream = await response.Content.ReadAsStreamAsync();
                AccessTokenResponse accessTokenResponse = await JsonSerializer.DeserializeAsync<AccessTokenResponse>(responseStream);
                return accessTokenResponse.AccessToken;
            }
            return "";
        }
    }
}