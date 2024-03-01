namespace MauiBlazorClient.Services
{
    internal class ModalDialogService : IModalDialogService
    {
        public event Action<string, string>? OnShow;

        public void Show(string title, string desctiption)
            => OnShow?.Invoke(title, desctiption);
    }
}
