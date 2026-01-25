namespace TipsWeb.Models
{
    //public class AppState
    //{
    //    public User SelectedProduct { get; set; }

    //    public event Action OnChange;

    //    public void SetProduct(User product)
    //    {
    //        SelectedProduct = product;
    //        NotifyStateChanged();
    //    }

    //    private void NotifyStateChanged() => OnChange?.Invoke();
    //}
    public class AppState
    {
        private User? _currentUser;

        public User? CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                NotifyStateChanged();
            }
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
