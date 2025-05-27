using TODO.Services;
using TODO.Models;
using Microsoft.Maui.Controls;
using System;

namespace TODO
{
    public partial class SignUpPage : ContentPage
    {
        private readonly ApiService _apiService = new ApiService();

        public SignUpPage()
        {
            InitializeComponent();
        }

        private async void OnSignInClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//MainPage");
        }

        private async void OnSignUpConfirmClicked(object sender, EventArgs e)
        {
            // Safely assign default empty strings to avoid null issues
            string firstName = FirstNameEntry.Text?.Trim() ?? string.Empty;
            string lastName = LastNameEntry.Text?.Trim() ?? string.Empty;
            string email = EmailEntry.Text?.Trim() ?? string.Empty;
            string password = PasswordEntry.Text ?? string.Empty;
            string confirmPassword = ConfirmPasswordEntry.Text ?? string.Empty;

            // Validation
            if (string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                await DisplayAlert("Error", "All fields are required.", "OK");
                return;
            }

            if (password != confirmPassword)
            {
                await DisplayAlert("Error", "Passwords do not match.", "OK");
                return;
            }

            var request = new SignUpRequest
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password,
                ConfirmPassword = confirmPassword
            };

            bool success = await _apiService.SignUpAsync(request);

            if (success)
            {
                await DisplayAlert("Success", "Account created successfully.", "OK");
                await Shell.Current.GoToAsync("TaskPage");
            }
            else
            {
                await DisplayAlert("Error", "Signup failed. Please try again.", "OK");
            }

        }
    }
}
