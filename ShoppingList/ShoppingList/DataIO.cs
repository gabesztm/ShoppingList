using ShoppingList.Resources;

namespace ShoppingList
{
    public partial class DataIO
    {
        private readonly string _fileName = "Data.shoppinglist";
        
        private List<string> _items = new List<string>();
        public IReadOnlyCollection<string> Items => _items.AsReadOnly();

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

        public DataIO()
        {
            Init();
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
            await Load(filename);
            var importedList = Items.ToList();
            if(action == ImportActions.Overwrite)
            {
                await Save(importedList);
                return;
            }

            if(action == ImportActions.Append)
            {
                await Load();

                foreach (var importedItem in importedList)
                {
                    _items.Add(importedItem);
                }

                var filteredList = _items.Distinct();
                await Save(filteredList);
                return;
            }
            return;
            
        }

        public ShareFileRequest GetShareFile()
        {
            return new ShareFileRequest()
            {
                Title = AppRes.ShareTitle,
                File = new ShareFile(GetPath())
            };
        }

        public async Task Load(string path = null)
        {
            string pathToLoad = string.IsNullOrEmpty(path) ? GetPath() : path;
            if (File.Exists(pathToLoad))
            {
                _items = new List<string>();
                foreach (var item in await File.ReadAllLinesAsync(pathToLoad))
                {
                    _items.Add(item);
                }
            }
            return;
        }

        private void Init()
        {
            if (File.Exists(GetPath()))
            {
                return;
            }
            using StreamWriter file = new StreamWriter(GetPath(), append: false);
        }

        private string GetPath()
        {
            return Path.Combine(FileSystem.Current.AppDataDirectory, _fileName);
        }
    }
}
