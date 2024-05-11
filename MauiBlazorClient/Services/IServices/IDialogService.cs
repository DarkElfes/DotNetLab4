
namespace MauiBlazorClient.Services.IServices;

public interface IDialogService
{
    event Action<string, string>? OnShowNotification;
    event Action<string, Type, Action<object?>>? OnShowForm;

    void ShowNotification(string title, string description);
    void ShowForm<TModel>(string title, Action<object?> onSubmit) where TModel : new();
}
