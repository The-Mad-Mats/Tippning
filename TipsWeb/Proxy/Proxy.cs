using System.Net.Http.Json;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TipsWeb
{
    public class Proxy
    {
        private readonly HttpClient _httpClient;

        public Proxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> GetFromApiAsync<T>(string url)
        {
            try
            {
                //_httpClient.BaseAddress = new Uri("https://localhost:44342/");
                var response = await _httpClient.GetFromJsonAsync<T>(url);
                return response;
            }
            catch (Exception ex)
            {
                // Handle exception
                throw new ApplicationException($"Error fetching data from API: {ex.Message}");
            }
        }
        public async Task<T> PostToApiAsync<T>(string url, T content)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(url, content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<T>();
            }
            catch (Exception ex)
            {
                // Handle exception
                throw new ApplicationException($"Error posting data to API: {ex.Message}");
            }
        }
    }
}
