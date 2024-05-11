
using Microsoft.AspNetCore.Components;

namespace MauiBlazorClient.Components.Base.Icons;

public record Icon(string Name, IconSize Size, string Content )
{
    public MarkupString ToMarkup()
    {
        var pxSize = $"{(int)Size}px";
        return new MarkupString($"<svg viewBox=\"0 0 {(int)Size} {(int)Size}\" width=\"{pxSize}\" height=\"{pxSize}\">{Content}</svg>");
    }
}
