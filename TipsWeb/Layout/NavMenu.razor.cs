using TipsWeb.Models;

namespace TipsWeb.Layout
{
    public partial class NavMenu
    {
        private User user = new();
        protected override async Task OnInitializedAsync()
        {
            AppState.OnChange += StateHasChanged;
            //user = AppState.CurrentUser;
        }
        public void Dispose()
        {
            AppState.OnChange -= StateHasChanged;
        }

        private bool collapseNavMenu = true;
        private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }

        //protected override void OnInitialized()
        //{
        //    AppState.OnChange += StateHasChanged;
        //}
    }
}