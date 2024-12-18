using Android.App;
using Android.Runtime;

namespace Netplanety.Maui.Platforms.Android;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(nint handle, JniHandleOwnership ownership) : base(handle, ownership)
    {

    }

    protected override MauiApp CreateMauiApp()
    {
        return MauiProgram.CreateMauiApp();
    }
}
