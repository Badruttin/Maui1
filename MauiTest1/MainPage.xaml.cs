namespace MauiTest1;

public partial class MainPage : ContentPage
{
    ApplicationViewModel viewModel;
    public MainPage()
    {
        InitializeComponent();
        viewModel = new ApplicationViewModel() { Navigation = this.Navigation };
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        await viewModel.GetVidRabot();
        base.OnAppearing();
    }
}


