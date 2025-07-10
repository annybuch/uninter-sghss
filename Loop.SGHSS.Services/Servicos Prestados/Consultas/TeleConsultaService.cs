using Loop.SGHSS.Extensions.Exceptions;
using Loop.SGHSS.Model._Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;

namespace Loop.SGHSS.Services.Servicos_Prestados.Consultas
{
    public class TeleConsultaService : ITeleConsultaService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public TeleConsultaService(IHttpClientFactory httpClientFactory, IOptions<LoopSGHSSConfiguration> config)
        {
            _httpClient = httpClientFactory.CreateClient();

            var dailyApiConfig = config.Value.DailyApi
                ?? throw new ArgumentException("Configuração 'DailyApi' não encontrada");

            if (string.IsNullOrWhiteSpace(dailyApiConfig.ApiUrl))
                throw new ArgumentException("Url da API Daily não configurada");

            if (string.IsNullOrWhiteSpace(dailyApiConfig.ApiKey))
                throw new ArgumentException("Chave da API Daily não configurada");

            _httpClient.BaseAddress = new Uri(dailyApiConfig.ApiUrl);
            _apiKey = dailyApiConfig.ApiKey;

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
        }

        /// <summary>
        /// Responsável por criar a sala de video chamada da teleconsulta, retornando o link autenticado com token.
        /// </summary>
        /// <param name="nomeSala"></param>
        /// <returns></returns>
        /// <exception cref="SGHSSBadRequestException"></exception>
        public async Task<string> CriarSala(string nomeSala)
        {
            var request = new
            {
                name = nomeSala,
                privacy = "private", 
                properties = new
                {
                    exp = DateTimeOffset.UtcNow.AddHours(2).ToUnixTimeSeconds(),
                    enable_chat = true,
                    enable_screenshare = true,
                    start_audio_off = false,
                    start_video_off = false,
                    enable_knocking = false
                }
            };

            var response = await _httpClient.PostAsJsonAsync("https://api.daily.co/v1/rooms/", request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new SGHSSBadRequestException($"Erro ao criar sala: {error}");
            }

            var content = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();
            return content["url"]?.ToString() ?? throw new SGHSSBadRequestException("URL da sala não encontrada.");
        }

        /// <summary>
        /// Responsável por gerar o token de acesso para criar a sala de video chamada.
        /// </summary>
        /// <param name="nomeSala"></param>
        /// <param name="nomeUsuario"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public string GerarTokenAcesso(string nomeSala, string nomeUsuario, string role = "user")
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_apiKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim("room", nomeSala),
                new Claim("iss", _apiKey),
                new Claim("user_name", nomeUsuario),
                new Claim("role", role)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Responsável por fazer o encerramento da sala de video chamada.
        /// </summary>
        /// <param name="nomeSala"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task EncerrarSala(string nomeSala)
        {
            var response = await _httpClient.DeleteAsync($"https://api.daily.co/v1/rooms/{nomeSala}/");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new SGHSSBadRequestException($"Erro ao encerrar sala: {error}");
            }
        }
    }
}
