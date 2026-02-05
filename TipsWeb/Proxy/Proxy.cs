using System.Net.Http.Json;
using TipsWeb.Models;
//using TipsWebApi.Models;
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
        public async Task<User> Login(LoginReq content)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Tips/Login", content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<User>();
            }
            catch (Exception ex)
            {
                // Handle exception
                throw new ApplicationException($"Error posting data to API: {ex.Message}");
            }
        }
        public async Task<bool> CreateUser(CreateUserReq content)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Tips/CreateUser", content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<bool>();
            }
            catch (Exception ex)
            {
                // Handle exception
                throw new ApplicationException($"Error posting data to API: {ex.Message}");
            }
        }
        public async Task<List<League>> GetUserleague(GetDefaultReq content)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Tips/GetUserLeagues", content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<League>>();
            }
            catch (Exception ex)
            {
                // Handle exception
                throw new ApplicationException($"Error posting data to API: {ex.Message}");
            }
        }
        public async Task<List<Game>> GetUserGames(GetDefaultReq content)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Tips/GetUserGames", content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<Game>>();
            }
            catch (Exception ex)
            {
                // Handle exception
                throw new ApplicationException($"Error posting data to API: {ex.Message}");
            }
        }
        public async Task<bool> SaveGames(SaveGamesReq content)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Tips/SaveGames", content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<bool>();
            }
            catch (Exception ex)
            {
                // Handle exception
                throw new ApplicationException($"Error posting data to API: {ex.Message}");
            }
        }
        public async Task<List<LeagueRow>> GetLeague(GetLeagueReq content)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Tips/GetLeague", content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<LeagueRow>>();
            }
            catch (Exception ex)
            {
                // Handle exception
                throw new ApplicationException($"Error posting data to API: {ex.Message}");
            }
        }
        public async Task<bool> CreateLeague(CreateOrJoinLeageReq content)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Tips/CreateLeague", content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<bool>();
            }
            catch (Exception ex)
            {
                // Handle exception
                throw new ApplicationException($"Error posting data to API: {ex.Message}");
            }
        }

        public async Task<bool> JoinLeague(CreateOrJoinLeageReq content)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Tips/JoinLeague", content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<bool>();
            }
            catch (Exception ex)
            {
                // Handle exception
                throw new ApplicationException($"Error posting data to API: {ex.Message}");
            }
        }

        public async Task<GameAdmin> AddGame(AddGameReq content)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Admin/AddGame", content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<GameAdmin>();
            }
            catch (Exception ex)
            {
                // Handle exception
                throw new ApplicationException($"Error posting data to API: {ex.Message}");
            }
        }
        public async Task<List<GameAdmin>> GetGames(GetGamesReq content)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Admin/GetGames", content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<GameAdmin>>();
            }
            catch (Exception ex)
            {
                // Handle exception
                throw new ApplicationException($"Error posting data to API: {ex.Message}");
            }
        }
        public async Task<bool> CalculateResul(CalcResultReq content)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Admin/CalculateREsult", content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<bool>();
            }
            catch (Exception ex)
            {
                // Handle exception
                throw new ApplicationException($"Error posting data to API: {ex.Message}");
            }
        }

    }
}
