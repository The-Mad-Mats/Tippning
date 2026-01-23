using Microsoft.AspNetCore.Components;
namespace TipsWeb.Layout;
public sealed class NavigationManagerFacade(NavigationManager manager) : INavigationManager
{
    private readonly NavigationManager _navigationManager = manager;
    public string BaseUri => manager?.BaseUri;
    public void NavigateTo(string uri, bool forceLoad = false)
    {
        if(_navigationManager.Uri == uri)
        {
            return;
        }
        _navigationManager.NavigateTo(uri, forceLoad);
    }
}