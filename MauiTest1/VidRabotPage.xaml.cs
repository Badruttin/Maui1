namespace MauiTest1;

public partial class VidRabotPage : ContentPage
{
    public VidRabot Model { get; private set; }
    public ApplicationViewModel ViewModel { get; private set; }
    public VidRabotPage(VidRabot model, ApplicationViewModel viewModel)
    {
        InitializeComponent();
        Model = model;
        ViewModel = viewModel;
        this.BindingContext = this;
    }
}
