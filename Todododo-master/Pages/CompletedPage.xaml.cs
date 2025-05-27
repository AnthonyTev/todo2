using TODO.Models;
using TODO.Services;

namespace TODO.Pages
{
    public partial class CompletedPage : ContentPage
    {
        private readonly ApiService _apiService;

        public CompletedPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var userId = Preferences.Get("user_id", 0);
            var tasks = await _apiService.GetTasksAsync(userId);
            CompletedCollectionView.ItemsSource = tasks?.Where(t => t.IsCompleted).ToList();
        }

        private async void OnDeleteTaskClicked(object sender, EventArgs e)
        {
            if ((sender as SwipeItem)?.CommandParameter is TaskModel task)
            {
                if (await DisplayAlert("Confirm", "Delete this task?", "Yes", "No"))
                {
                    await _apiService.DeleteTaskAsync(task.Id);
                    OnAppearing(); // refresh list
                }
            }
        }

        private async void GotoEditPage(object sender, EventArgs e)
        {
            if ((sender as SwipeItem)?.CommandParameter is TaskModel task)
            {
                await Shell.Current.GoToAsync($"{nameof(EditPage)}?taskId={task.Id}");
            }
        }
    }
}


