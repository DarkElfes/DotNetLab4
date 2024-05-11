using MauiBlazorClient.Components.Base.Icons;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace MauiBlazorClient.Components.Base.Button;

public partial class Button
{
    private bool _isClicked;

    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
    [Parameter] public ButtonType? Type { get; set; } = ButtonType.Button;
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public Icon? IconStart { get; set; }
    [Parameter] public Icon? IconEnd { get; set; }


    protected async Task OnClickHandlerAsync(MouseEventArgs e)
    {
        if (!Disabled)
        {
            _isClicked = true;
            await Task.Delay(200);
            _isClicked = false;

            if(OnClick.HasDelegate)
                await OnClick.InvokeAsync(e);
        }

        await Task.CompletedTask;
    }

    protected string? GetClasses()
    {
        string? classes = string.Join(' ', [ChildContent is null ? "only-icon" : null, _isClicked ? "clicked" : null]);
        
        if (classes?.Last().Equals(' ') ?? false)
            classes = classes.Remove(classes.Length - 1);

        return classes;
    }
}
