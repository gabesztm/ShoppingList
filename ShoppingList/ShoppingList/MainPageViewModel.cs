﻿using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShoppingList.Resources;
using System.Diagnostics;

namespace ShoppingList
{
    public partial class MainPageViewModel : ObservableObject
    {
        private readonly DataIO _dataIO;

        [ObservableProperty]
        private ObservableCollection<string> _itemList;


        public MainPageViewModel(DataIO dataIO)
        {
            _dataIO = dataIO;
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
            var t = Task.Run(async () =>
            {
                var shareFileRequest = _dataIO.GetShareFile();
                await Microsoft.Maui.ApplicationModel.DataTransfer.Share.Default.RequestAsync(shareFileRequest);
            });
            t.Wait();
        }

        [RelayCommand]
        public async Task OpenFileLocation()
        {
#if WINDOWS || MACCATALYST
            await _dataIO.OpenFileLocation();
#endif
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

                await _dataIO.ImportFromFile(result.FullPath, (ImportActions)action);
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

            ItemList.Remove(item);
            await _dataIO.Save(ItemList);
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
            LoadFromDisk();
        }

        private void LoadFromDisk()
        {
            ObservableCollection<string> shoppingList = new ObservableCollection<string>();
            var t = Task.Run(async () => {
                await _dataIO.Load();
                foreach (var item in _dataIO.Items)
                {
                    shoppingList.Add(item);
                }
            });
            t.Wait();
            ItemList = shoppingList;
        }
    }

}
