using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TipsWeb.Pages
{
    public partial class Weather
    {
        [Inject] public Proxy _proxy {  get; set; }

        public WeatherForecast[]? forecasts;

        protected override async Task OnInitializedAsync()
        {
            forecasts = await _proxy.GetFromApiAsync<WeatherForecast[]>("WeatherForecast");
        }
    }
}
