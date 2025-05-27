
using TODO.Models;
using TODO.Services;
using Microsoft.Maui.Controls;

namespace TODO.Pages
{
    [QueryProperty(nameof(TaskId), "taskId")]
    public partial class EditPage : ContentPage
    {
        public int TaskId { get; set; }
        private TaskModel? _task;
        private readonly ApiService _apiService;

        public EditPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _task = await _apiService.GetTaskByIdAsync(TaskId);
            if (_task != null)
            {
                TitleEntry.Text = _task.Title;
                DescriptionEntry.Text = _task.Description;
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (_task != null)
            {
                _task.Title = TitleEntry.Text ?? _task.Title;
                _task.Description = DescriptionEntry.Text ?? _task.Description;
                await _apiService.UpdateTaskAsync(_task);
                await Shell.Current.GoToAsync("..");
            }
        }
    }
}
