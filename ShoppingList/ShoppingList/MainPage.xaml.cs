namespace ShoppingList;

public partial class MainPage : ContentPage
{
	private readonly MainPageViewModel _viewModel;

	public MainPage()
	{
		InitializeComponent();
		Init();
		_viewModel = new MainPageViewModel();
		BindingContext = _viewModel;
	}

    protected override void OnAppearing()
    {
		_viewModel.OnAppearing();
        base.OnAppearing();
    }

	private void Init()
	{
#if (WINDOWS || MACCATALYST)
		buttonsGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
		foreach (var child in buttonsGrid.Children)
		{
			int currentColumn = buttonsGrid.GetColumn(child);

            if (currentColumn > 1)
			{
				buttonsGrid.SetColumn(child, currentColumn + 1);
			}
		}

		var openLocationImageButton = new ImageButton() { Source = "folder_svgrepo_com.png" };
		openLocationImageButton.SetBinding(ImageButton.CommandProperty, new Binding("OpenFileLocationCommand"));
		buttonsGrid.Children.Add(openLocationImageButton);
		buttonsGrid.SetColumn(openLocationImageButton, 2);
#endif
    }

}

