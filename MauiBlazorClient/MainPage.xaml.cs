namespace MauiBlazorClient;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        Loaded += LoadingHandler;
    }

    private async void LoadingHandler(object? s, EventArgs e)
    {
        //var viewHandler = blazorWebView.Handler;

        try
        {

            var webView2 = (blazorWebView.Handler.PlatformView as Microsoft.UI.Xaml.Controls.WebView2);
            await webView2.EnsureCoreWebView2Async();
            webView2.DefaultBackgroundColor = Windows.UI.Color.FromArgb(255, 0, 0, 0);

            var settings = webView2.CoreWebView2.Settings;
            settings.IsZoomControlEnabled = false;
            settings.IsGeneralAutofillEnabled = false;

#if WINDOWS && RELEASE
        settings.AreBrowserAcceleratorKeysEnabled = false;
#endif
        }
        catch
        {

        }
    }

}
