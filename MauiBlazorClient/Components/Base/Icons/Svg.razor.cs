
using Microsoft.AspNetCore.Components;

namespace MauiBlazorClient.Components.Base.Icons;

public partial class Svg
{
    [Parameter, EditorRequired]
    public Icon Value { get; set; } = default!;
}
