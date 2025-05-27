using TODO.Models;
using TODO.Services;

namespace TODO.Pages
{
    public partial class EditPage : ContentPage
    {
        private readonly ApiService _apiService;
        private TaskModel? _task;

        public EditPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            var uri = Shell.Current.CurrentState.Location;
            var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
            var taskId = query["taskId"];
            if (!string.IsNullOrEmpty(taskId))
            {
                int id = int.Parse(taskId);
                _task = await _apiService.GetTaskByIdAsync(id);
                // Fill UI inputs with _task.Title, _task.Description, etc.
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            // Assume bound to inputs
            if (_task != null)
            {
                await _apiService.UpdateTaskAsync(_task);
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                // Optionally handle the null case, e.g., show an error message
            }
        }
    }
}
