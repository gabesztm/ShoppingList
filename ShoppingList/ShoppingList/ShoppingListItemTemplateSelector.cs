namespace ShoppingList
{
    internal class ShoppingListItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Template { get; set; }
        public DataTemplate WindowsTemplate { get; set; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if(DeviceInfo.Current.Platform == DevicePlatform.WinUI)
            {
                return WindowsTemplate;
            }
            return Template;
        }
    }
}
