namespace MauiBlazorClient.Services.IServices;

public interface IModalDialogService
{
    event Action<string, string>? OnShow;

    void Show(string title, string description);
}
