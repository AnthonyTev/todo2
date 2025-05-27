using System.Collections.ObjectModel;
using TODO.Models;
using TODO.Services;

namespace TODO.Pages
{
    public partial class TaskPage : ContentPage
    {
        private readonly ApiService _apiService;
        private List<TaskModel> _tasks = new();

        public ObservableCollection<TaskItem> Tasks { get; set; }

        public TaskPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
            Tasks = new ObservableCollection<TaskItem>
            {
                new TaskItem { Title = "Buy groceries" },
                new TaskItem { Title = "Walk the dog" },
                new TaskItem { Title = "Finish project" }
            };
            BindingContext = this;
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

        private static async void OnEditTaskClicked(object sender, EventArgs e)
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

        private void OnMoveUpClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var task = button?.BindingContext as TaskItem;
            if (task == null)
                return;
            int index = -1;
            if (task != null)
                index = Tasks.IndexOf(task);
            if (index > 0)
            {
                Tasks.Move(index, index - 1);
            }
        }

        private void OnMoveDownClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var task = button?.BindingContext as TaskItem;
            if (task == null)
                return;
            var index = Tasks.IndexOf(task);
            if (index < Tasks.Count - 1 && index >= 0)
            {
                Tasks.Move(index, index + 1);
            }
        }

        private void OnDeleteTaskItemClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var task = button?.BindingContext as TaskItem;
            if (task != null)
                Tasks.Remove(task);
        }
    }

    public class TaskItem
    {
        public string? Title { get; set; }
        public bool IsCompleted { get; set; }
    }
}



