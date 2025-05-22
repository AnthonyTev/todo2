using TODO.Services;

namespace TODO
{
    public partial class MainPage : ContentPage
    {
        private readonly ApiService _apiService;

        public MainPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        private async void OnSignInClicked(object sender, EventArgs e)
        {
            string email = EmailEntry.Text?.Trim();
            string password = PasswordEntry.Text?.Trim();

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "Please enter email and password.", "OK");
                return;
            }

            var result = await _apiService.SignInAsync(email, password);

            if (result.Status == 200)
            {
                await DisplayAlert("Success", $"Welcome {result.Data.FirstName}!", "Continue");

                // Store the user ID to App.Current.Properties or a static variable
                Preferences.Set("user_id", result.Data.Id);
                

                // Navigate to the task page
                await Shell.Current.GoToAsync(nameof(Pages.TaskPage));
            }   
            else
            {
                await DisplayAlert("Login Failed", result.Message, "OK");
            }
        }

        private async void OnSignUpClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(TODO.SignUpPage));
        }

        private void OnForgotPasswordTapped(object sender, TappedEventArgs e)
        {
            // Optional: Navigate to a forgot password page or alert
            DisplayAlert("Info", "Forgot password feature coming soon.", "OK");
        }
    }
}