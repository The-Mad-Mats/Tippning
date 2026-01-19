namespace TipsWeb.Models
{
    public class AppState
    {
        public User SelectedProduct { get; set; }

        public event Action OnChange;

        public void SetProduct(User product)
        {
            SelectedProduct = product;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
