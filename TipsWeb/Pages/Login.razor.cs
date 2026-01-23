using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using TipsWeb.Models;

namespace TipsWeb.Pages
{
    public partial class Login
    {
        [Inject] public Proxy _proxy { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }

        private LoginModel loginModel = new ();

        private string errorMessage = string.Empty;

        private bool isLoading = false;

        private async Task HandleLogin()
        {
            isLoading = true;
            errorMessage = string.Empty;
            try
            {
                var loginReq = new LoginReq
                {
                    Username = loginModel.Email,
                    Password = loginModel.Password
                };
                var user = await _proxy.Login(loginReq);

                if (user.Id != 0)
                {
                    AppState.SetProduct(user);
                    NavigationManager.NavigateTo("/");
                }
                else
                {
                    errorMessage = "Invalid email or password";
                }
            }
            catch(Exception ex)
            {
                errorMessage = "An error occurred during login. Please try again.";
            }
            finally
            {
                isLoading = false;
            }
        }
        public class LoginModel
        {
            [Required(ErrorMessage = "Email is required")]
            //[EmailAddress(ErrorMessage = "Invalid email address")]
            public string Email {get; set;} = string.Empty;

            [Required(ErrorMessage = "Password is required")]
            //[MinLength(6,ErrorMessage = "Password must be at least 6 characters")]
            public string Password {get; set;} = string.Empty;

            public bool RememberMe {get; set;}
        }
    }
}
