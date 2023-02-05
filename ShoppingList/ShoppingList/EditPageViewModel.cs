using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text;

namespace ShoppingList
{
    public partial class EditPageViewModel : ObservableObject
    {
        private readonly DataIO _dataIO;

        [ObservableProperty]
        private string _shoppingItemList;

        public EditPageViewModel(DataIO dataIO)
        {
            _dataIO = dataIO;
        }

        public void OnAppearing()
        {
            var t = Task.Run(async () => {
                StringBuilder sb = new StringBuilder();
                await _dataIO.Load();
                foreach (var line in _dataIO.Items)
                {
                    sb.AppendLine(line);
                }
                ShoppingItemList = sb.ToString();
            });
            t.Wait();
        }

        [RelayCommand]
        public async Task Save()
        {
            await _dataIO.Save(ShoppingItemList);
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        public async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
