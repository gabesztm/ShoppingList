using System.Diagnostics;

namespace ShoppingList
{
    public partial class DataIO
    {
        public async Task OpenFileLocation()
        {
            await Task.Run(() =>
            {
                Process.Start("explorer.exe", FileSystem.Current.AppDataDirectory);
            });
        }
    }
}
