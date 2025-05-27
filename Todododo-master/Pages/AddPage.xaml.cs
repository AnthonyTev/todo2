
using TODO.Models;
using TODO.Services;

namespace TODO.Pages
{
    public partial class AddPage : ContentPage
    {
        private readonly ApiService _apiService;

        public AddPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        private async void OnAddClicked(object sender, EventArgs e)
        {
            var title = TitleEntry.Text?.Trim() ?? string.Empty;
            var description = DescriptionEntry.Text?.Trim() ?? string.Empty;

            var task = new TaskModel
            {
                Title = title,
                Description = description,
                UserId = SessionData.UserId
            };

            bool success = await _apiService.AddTaskAsync(task);

            if (success)
            {
                await DisplayAlert("Success", "Task added.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Failed to add task.", "OK");
            }
        }
    }
}
