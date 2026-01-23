namespace TipsWeb.Layout
{
    public interface INavigationManager
    {
        string BaseUri { get; }
        void NavigateTo(string uri, bool forceLoad = false);
    }
}
