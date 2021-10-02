using Microsoft.Extensions.Configuration;
using PlatformService.Dtos;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlatformService.SyncDataService.Http
{
    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpCommandDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            this._httpClient = httpClient;
            this._configuration = configuration;
        }
        public async Task SendPlatformToCommand(PlatformReadDto platform)
        {
            var httpContent = new StringContent
            (
                JsonSerializer.Serialize(platform),
                Encoding.UTF8,
                "application/json"
            );

            string cmdUrl = $"{this._configuration["CommandServiceBaseUrl"]}/api/c/Platforms";
            var response = await this._httpClient.PostAsync(cmdUrl, httpContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync POST to CommandService was OK!");
            }
            else
            {
                Console.WriteLine("-->Sync POST to CommandService was NOT OK!");
            }
        }
    }
}
