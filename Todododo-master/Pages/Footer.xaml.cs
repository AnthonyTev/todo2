
namespace TODO.Pages;

public partial class Footer : ContentView
{
	public Footer()
	{
		InitializeComponent();
	}

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("TaskPage");
    }

    private async void ImageButton_Clicked_1(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("CompletedPage");
    }

    private async void ImageButton_Clicked_2(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("Profile");
    }
}