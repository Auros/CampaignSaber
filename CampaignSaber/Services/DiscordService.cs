using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Collections.Generic;
using CampaignSaber.Models.Discord;
using CampaignSaber.Models.Settings;

namespace CampaignSaber.Services
{
    public class DiscordService
    {
        private const string _discordUserURLString = "https://discordapp.com/api/users/@me";
        private const string _discordAuthURLString = "https://discord.com/api/oauth2/token";

        private readonly HttpClient _client;

        public string ID { get; private set; }
        public string Secret { get; private set; }
        public string RedirectURL { get; private set; }

        public DiscordService(HttpClient client, IDiscordSettings settings)
        {
            _client = client;

            ID = settings.ID;
            Secret = settings.Secret;
            RedirectURL = settings.RedirectURL;
        }

        public async Task<string> GetAccessToken(string code)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "client_id", ID },
                { "client_secret", Secret },
                { "grant_type", "authorization_code" },
                { "code", code },
                { "redirect_uri", RedirectURL }
            };
            FormUrlEncodedContent content = new FormUrlEncodedContent(parameters);
            HttpResponseMessage response = await _client.PostAsync(_discordAuthURLString, content);
            if (response.IsSuccessStatusCode)
            {
                Stream responseStream = await response.Content.ReadAsStreamAsync();
                AccessTokenResponse accessTokenResponse = await JsonSerializer.DeserializeAsync<AccessTokenResponse>(responseStream);
                return accessTokenResponse.AccessToken;
            }
            return null;
        }

        public async Task<DiscordUser> GetProfile(string accessToken)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            HttpResponseMessage response = await _client.GetAsync(_discordUserURLString);
            if (response.IsSuccessStatusCode)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                DiscordUser discordUser = JsonSerializer.Deserialize<DiscordUser>(responseString);
                return discordUser;
            }
            return null;
        }
    }
}