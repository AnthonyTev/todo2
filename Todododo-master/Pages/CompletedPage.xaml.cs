namespace TODO.Pages;

public partial class CompletedPage : ContentPage
{
	public CompletedPage()
	{
		InitializeComponent();
	}
    private async void GotoEditPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("EditCompletedPage");
    }
}