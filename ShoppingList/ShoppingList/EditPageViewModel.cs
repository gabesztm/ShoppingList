using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text;

namespace ShoppingList
{
    public partial class EditPageViewModel : ObservableObject
    {
        private readonly DataIO _dataIO;

        [ObservableProperty]
        private string _shoppingList;

        public EditPageViewModel(DataIO dataIO)
        {
            _dataIO = dataIO;
        }

        public void OnAppearing()
        {
            var t = Task.Run(async () => {
                StringBuilder sb = new StringBuilder();
                foreach (var line in await _dataIO.Load())
                {
                    sb.AppendLine(line);
                }
                ShoppingList = sb.ToString();
            });
            t.Wait();
        }

        [RelayCommand]
        public async Task Save()
        {
            await _dataIO.Save(ShoppingList);
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        public async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
