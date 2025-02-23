
namespace MSWindowsAppLauncher.M_MSWindowsAppLauncher;

internal sealed class M_MSWindowsAppLauncher_ClientApp
{
    private static async Task Main(String[] in_args) 
        => await AppLaunchRunner.S_ExecuteLaunchingApps();   
}