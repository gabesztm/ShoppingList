namespace ShoppingList;

public partial class MainPage : ContentPage
{
	private readonly MainPageViewModel _viewModel;

	public MainPage()
	{
		InitializeComponent();
		_viewModel = new MainPageViewModel();
		BindingContext = _viewModel;
	}

    protected override void OnAppearing()
    {
		_viewModel.OnAppearing();
        base.OnAppearing();
    }

}

