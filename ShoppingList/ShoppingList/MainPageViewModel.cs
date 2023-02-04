using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShoppingList.Resources;

namespace ShoppingList
{
    internal partial class MainPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<string> _list;


        public MainPageViewModel()
        {
            Init();
        }

        [RelayCommand]
        public async Task Edit()
        {
            await Shell.Current.GoToAsync(nameof(EditPage));
        }

        [RelayCommand]
        public void Share()
        {
            var t = Task.Run( () => {  DataIO.Share(); });
            t.Wait();
        }

        [RelayCommand]
        public void OpenFileLocation()
        {
            DataIO.OpenFileLocation();
        }

        [RelayCommand]
        public async Task Import()
        {
            var result = await FilePicker.Default.PickAsync();
            if(result != null && result.FileName.EndsWith("shoppinglist", StringComparison.InvariantCultureIgnoreCase)) 
            {
                var actionString = await Shell.Current.DisplayActionSheet(AppRes.ActionTitle, AppRes.Cancel, null, AppRes.ActionOverwrite, AppRes.ActionAppend);
                var action = GetImportAction(actionString);
                if(action == null)
                {
                    return;
                }

                await DataIO.ImportFromFile(result.FullPath, (ImportActions)action);
                LoadFromDisk();
            }
        }

        [RelayCommand]
        private async Task Select(object o)
        {
            string item = o as string;
            if(item == null)
            {
                return;
            }

            var confirmationAnswer = await Shell.Current.DisplayAlert(item, AppRes.ConfirmationQuestion, AppRes.Yes, AppRes.No );
            if (!confirmationAnswer)
            {
                return;
            }

            List.Remove(item);
            await DataIO.Save(List);
        }

        private ImportActions? GetImportAction(string actionString)
        {
            if (string.Compare(actionString, AppRes.ActionOverwrite) == 0)
            {
                return ImportActions.Overwrite;
            }

            if (string.Compare(actionString, AppRes.ActionAppend) == 0)
            {
                return ImportActions.Append;
            }

            return null;
        }

        public void OnAppearing()
        {
           LoadFromDisk();
        }

        private void Init()
        {
            DataIO.CreateDefaultFile();
            LoadFromDisk();

        }

        private void LoadFromDisk()
        {
            ObservableCollection<string> shoppingList = new ObservableCollection<string>();
            var t = Task.Run(async () => {
                foreach (var item in await DataIO.Load())
                {
                    shoppingList.Add(item);
                }
            });
            t.Wait();
            List = shoppingList;
        }
    }

}
