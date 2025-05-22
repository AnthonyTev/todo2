namespace TODO.Pages;

public partial class Profile : ContentPage
{
	public Profile()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }
}