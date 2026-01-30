using Microsoft.AspNetCore.Components;

namespace TipsWeb.Pages
{
    public partial class Register
    {
        // ========================================
        // FIELDS
        // ========================================
        [Inject] public Proxy Proxy { get; set; }
        public UserModel userModel = new ();
        private List<UserModel> addedUsers = new();
        private string successMessage = string.Empty;
        private string errorMessage = string.Empty;
        private bool isLoading = false;

        // ========================================
        // LIFECYCLE METHODS
        // ========================================
        protected override void OnInitialized()
        {
            // No initialization needed
        }


        // ========================================
        // EVENT HANDLERS
        // ========================================
        private async Task HandleSubmit()
        {
            isLoading = true;
            errorMessage = string.Empty;
            successMessage = string.Empty;
            try
            {
                var createUserReq = new Models.CreateUserReq
                {
                    UserName = userModel.Username,
                    Password = userModel.Password,
                    Team = userModel.Team
                };
                var status = await Proxy.CreateUser(createUserReq);
                if (status)
                {
                    // Add user to list
                    addedUsers.Add(new UserModel
                    {
                        Username = userModel.Username,
                        Password = userModel.Password,
                        Team = userModel.Team
                    });

                    successMessage = $"User '{userModel.Username}' added successfully!";

                    // Reset form after successful submission
                    await Task.Delay(1500);
                    ResetForm();
                }
            }
            catch(Exception ex)
            {
                errorMessage = "An error occurred while adding the user. Please try again.";
            }
            finally
            {
                isLoading = false;
            }
        }
        private void ResetForm()
        {
            userModel = new UserModel();
            successMessage = string.Empty;
            errorMessage = string.Empty;
        }

        private void RemoveUser(UserModel user)
        {
            addedUsers.Remove(user);
        }

        // ========================================
        // MODELS
        // ========================================
        public class UserModel
        {
            public string Username { get; set; } = string.Empty;
            public string Password {get; set; } = string.Empty;
            public string Team { get; set; } = string.Empty;
        }
    }
}
