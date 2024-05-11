using Microsoft.AspNetCore.Components;

namespace MauiBlazorClient.Components.Base.ErrorMessage;

public partial class ErrorMessage
{
    /*private bool _isHidden = true;*/

    [Parameter] public string? Message { get; set; }

    /*protected override async Task OnParametersSetAsync()
    {
        _isHidden = false;
        await Task.Delay(2000);
        _isHidden = true;
    }*/
}
