using ShoppingList.Resources;

namespace ShoppingList
{
    public class DataIO
    {
        private readonly string _fileName = "Data.shoppinglist";


        public async Task Save(IEnumerable<string> data)
        {
            using (StreamWriter file = new StreamWriter(GetPath(), append: false))
            {
                foreach (var item in data)
                {
                    if(string.IsNullOrEmpty(item) || string.IsNullOrWhiteSpace(item)) continue;
                    await file.WriteLineAsync(item);
                }
            }
        }

        public async Task Save(string data)
        {
            List<string> list = new List<string>();
            using(StringReader sr = new StringReader(data)) 
            {
                string line;
                while ((line = await sr.ReadLineAsync()) != null)
                {
                    list.Add(line);
                }
            }
            await Save(list);
        }

        public async Task ImportFromFile(string filename, ImportActions action)
        {
            var importedList = await Load(filename);
            if(action == ImportActions.Overwrite)
            {
                await Save(importedList);
                return;
            }

            if(action == ImportActions.Append)
            {
                var alreadyAvailableList = await Load();

                foreach (var importedItem in importedList)
                {
                    alreadyAvailableList = alreadyAvailableList.Append(importedItem);
                }

                var filteredList = alreadyAvailableList.Distinct();
                await Save(filteredList);
                return;
            }
            return;
            
        }

        public void CreateDefaultFile()
        {
            if (File.Exists(GetPath()))
            {
                return;
            }
            using StreamWriter file = new StreamWriter(GetPath(), append: false);
        }

        public void Share()
        {
            var t = Task.Run(async () =>
            {
                await Microsoft.Maui.ApplicationModel.DataTransfer.Share.Default.RequestAsync(new ShareFileRequest()
                {
                    Title = AppRes.ShareTitle,
                    File = new ShareFile(GetPath())
                });
            });
            t.Wait();
        }

        public void OpenFileLocation()
        {
#if WINDOWS
            Process.Start("explorer.exe", FileSystem.Current.AppDataDirectory);
            return;
#elif MACCATALYST
            Process.Start("open", $"-R \"{FileSystem.Current.AppDataDirectory}\"");
            return;
#endif
        }


        public async Task<IEnumerable<string>> Load(string path = null)
        {
            string pathToLoad = string.IsNullOrEmpty(path) ? GetPath() : path;
            if (File.Exists(pathToLoad))
            {
                return await File.ReadAllLinesAsync(pathToLoad);
            }
            return Array.Empty<string>();
        }

        private string GetPath()
        {
            return Path.Combine(FileSystem.Current.AppDataDirectory, _fileName);
        }
    }
}
