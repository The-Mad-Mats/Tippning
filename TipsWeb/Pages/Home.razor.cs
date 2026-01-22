using TipsWeb.Models;

namespace TipsWeb.Pages
{
    public partial class Home
    {
        public User user = new();
        protected override void OnInitialized()
        {
            user = AppState.SelectedProduct;
        }
    }
}
