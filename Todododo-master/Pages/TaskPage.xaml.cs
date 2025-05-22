using TODO.Services;
using Microsoft.Maui.Controls;
using System;
using System.Linq;

namespace TODO.Pages;

public partial class TaskPage : ContentPage
{
    private readonly ApiService _apiService = new ApiService();

    public TaskPage()
    {
        InitializeComponent();
        LoadTasks(); // Load tasks when page is initialized
    }

    private async void LoadTasks()
    {
        try
        {
            var result = await _apiService.GetTasksAsync("active", SessionData.UserId);

            if (result.Status == 200 && result.Data?.Any() == true)
            {
                TaskListContainer.Children.Clear(); // Clear existing items

                foreach (var task in result.Data.Values)
                {
                    var swipeView = new SwipeView
                    {
                        RightItems = new SwipeItems
                        {
                            new SwipeItem
                            {
                                BackgroundColor = Colors.Red,
                                IconImageSource = "trash.png",
                                Command = new Command(async () => await DeleteTask(task.ItemId))
                            },
                            new SwipeItem
                            {
                                BackgroundColor = Color.FromArgb("#F0AD4E"),
                                IconImageSource = "edit.png",
                                Command = new Command(() => GotoEditPage(task.ItemId))
                            },
                            new SwipeItem
                            {
                                BackgroundColor = Colors.Green,
                                IconImageSource = "check.png",
                                Command = new Command(async () => await MarkAsDone(task.ItemId))
                            }
                        },
                        Content = new Frame
                        {
                            BackgroundColor = Color.FromArgb("#D6E9E5"),
                            BorderColor = Colors.Transparent,
                            CornerRadius = 10,
                            Padding = 10,
                            HasShadow = true,
                            Margin = new Thickness(20, 0, 20, 0),
                            Content = new Label
                            {
                                Text = task.ItemName,
                                FontSize = 16,
                                TextColor = Color.FromArgb("#74968F"),
                                VerticalOptions = LayoutOptions.Center
                            }
                        }
                    };

                    TaskListContainer.Children.Add(swipeView);
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load tasks: {ex.Message}", "OK");
        }
    }

    private async Task DeleteTask(int itemId)
    {
        var confirmed = await DisplayAlert("Confirm", "Are you sure you want to delete this task?", "Yes", "No");
        if (!confirmed) return;

        var response = await _apiService.DeleteTaskAsync(itemId);
        if (response.Status == 200)
        {
            LoadTasks(); // Refresh tasks
        }
        else
        {
            await DisplayAlert("Error", response.Message, "OK");
        }
    }

    private async Task MarkAsDone(int itemId)
    {
        var response = await _apiService.ChangeTaskStatusAsync(itemId, "inactive");
        if (response.Status == 200)
        {
            LoadTasks(); // Refresh tasks
        }
        else
        {
            await DisplayAlert("Error", response.Message, "OK");
        }
    }

    private async void GotoAddPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("AddPage");
    }

    private async void GotoEditPage(int itemId)
    {
        await Shell.Current.GoToAsync($"EditPage?itemId={itemId}");
    }
}
