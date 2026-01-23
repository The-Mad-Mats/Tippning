using TipsWeb.Models;

namespace TipsWeb.Layout
{
    public partial class NavMenu
    {
        private User user = new();
        protected override async Task OnInitializedAsync()
        {
            user = AppState.SelectedProduct;
        }
    }
}