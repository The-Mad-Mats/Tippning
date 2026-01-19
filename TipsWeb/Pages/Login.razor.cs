using System.ComponentModel.DataAnnotations;

namespace TipsWeb.Pages
{
    public partial class Login
    {
        private LoginModel loginModel = new ();

        private string errorMessage = string.Empty;

        private bool isLoading = false;

        private async Task HandleLogin()
        {
            isLoading = true;
            errorMessage = string.Empty;
            try
            {
                // Add your authentication logic here
                // Example: await AuthService.LoginAsync(loginModel.Email, loginModel.Password);
                // Simulate API call
                await Task.Delay(1000);

                // For demo purposes - replace with actual authentication
                if(loginModel.Email == "mats" && loginModel.Password == "password")
                {
                    // Navigate to home or dashboard
                    // NavigationManager.NavigateTo("/");
                    var user = new Models.User
                    {
                        UserName = "mats",
                        Password = "password",
                        Id = 1,
                        Token = "xyz"
                    };
                    AppState.SetProduct(user);
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
            [MinLength(6,ErrorMessage = "Password must be at least 6 characters")]
            public string Password {get; set;} = string.Empty;

            public bool RememberMe {get; set;}
        }
    }
}
