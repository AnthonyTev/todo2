using TODO.Services;
using TODO.Models;
namespace TODO.Pages;

public partial class AddPage : ContentPage
{
    private readonly ApiService _apiService = new ApiService();

    public AddPage()
    {
        InitializeComponent();
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        string title = TitleEntry.Text?.Trim() ?? "";
        string description = Description.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(description))
        {
            await DisplayAlert("Error", "Please enter both title and description.", "OK");
            return;
        }

        int userId = SessionData.UserId;

        var task = new TaskModel
        {
            Title = title,
            Description = description,
            UserId = userId
        };

        var newTask = await _apiService.AddTaskAsync(task);

        if (newTask)
        {
            await DisplayAlert("Success", "Task added successfully.", "OK");
            await Navigation.PopAsync(); // Go back to the previous page
        }
        else
        {
            await DisplayAlert("Error", "Failed to add task.", "OK");
        }
    }
}