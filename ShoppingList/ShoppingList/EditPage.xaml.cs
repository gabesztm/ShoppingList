namespace ShoppingList;

public partial class EditPage : ContentPage
{
	private readonly EditPageViewModel _viewModel;
	public EditPage(EditPageViewModel vm)
	{
		InitializeComponent();
		_viewModel = vm;
		BindingContext = _viewModel;
	}

    protected override void OnAppearing()
    {
		_viewModel.OnAppearing();
        base.OnAppearing();
    }
}