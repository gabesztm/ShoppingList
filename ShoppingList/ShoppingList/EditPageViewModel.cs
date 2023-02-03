using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text;

namespace ShoppingList
{
    public partial class EditPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _shoppingList;

        public void OnAppearing()
        {
            var t = Task.Run(async () => {
                StringBuilder sb = new StringBuilder();
                foreach (var line in await DataIO.Load())
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
            await DataIO.Save(ShoppingList);
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        public async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
