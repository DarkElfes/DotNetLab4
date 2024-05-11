
namespace MauiBlazorClient.Services;

public class DialogService : IDialogService
{
    public event Action<string, string>? OnShowNotification;
    public event Action<string, Type, Action<object?>>? OnShowForm;

    public void ShowNotification(string title, string desctiption)
        => OnShowNotification?.Invoke(title, desctiption);

    public void ShowForm<TModel>(string title, Action<object?> onSubmit) where TModel : new()
        => OnShowForm?.Invoke(title, typeof(TModel), onSubmit);
}