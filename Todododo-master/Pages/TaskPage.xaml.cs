using TODO.Models;
using TODO.Services;

namespace TODO.Pages
{
    public partial class TaskPage : ContentPage
    {
        private readonly ApiService _apiService;
        private List<TaskModel> _tasks = new();

        public TaskPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadTasks();
        }

        private async Task LoadTasks()
        {
            var userId = Preferences.Get("user_id", 0);
            if (userId == 0)
            {
                await DisplayAlert("Error", "User not logged in.", "OK");
                return;
            }

            _tasks = await _apiService.GetTasksAsync(userId) ?? new List<TaskModel>();
            TaskCollectionView.ItemsSource = _tasks;
        }

        private async void OnDeleteTaskClicked(object sender, EventArgs e)
        {
            if ((sender as Button)?.BindingContext is TaskModel task)
            {
                bool confirm = await DisplayAlert("Confirm", "Delete this task?", "Yes", "No");
                if (confirm)
                {
                    await _apiService.DeleteTaskAsync(task.Id);
                    await LoadTasks();
                }
            }
        }

        private async void OnEditTaskClicked(object sender, EventArgs e)
        {
            if ((sender as Button)?.BindingContext is TaskModel task)
            {
                await Shell.Current.GoToAsync($"{nameof(EditPage)}?taskId={task.Id}");
            }
        }

        private async void OnCompletedChanged(object sender, CheckedChangedEventArgs e)
        {
            if ((sender as CheckBox)?.BindingContext is TaskModel task)
            {
                task.IsCompleted = e.Value;
                await _apiService.UpdateTaskAsync(task);

                if (task.IsCompleted)
                    await Shell.Current.GoToAsync(nameof(CompletedPage));
                else
                    await LoadTasks();
            }
        }

        private async void OnReorderCompleted(object sender, EventArgs e)
        {
            var reorderedTasks = TaskCollectionView.ItemsSource?.Cast<TaskModel>().ToList();
            if (reorderedTasks == null)
                return;

            for (int i = 0; i < reorderedTasks.Count; i++)
                reorderedTasks[i].Order = i;

            await Task.WhenAll(reorderedTasks.Select(task => _apiService.UpdateTaskOrderAsync(task)));
        }
    }
}



