
namespace MSWindowsAppLauncher.M_MSWindowsAppLauncher;

internal class AppLaunchModel
{
    internal string Path { get; set; }
    internal bool IsAdminRequired { get; set; }
    internal DateTime ExecutionTime { get; set; }

    internal AppLaunchModel(string path, bool isAdminRequired)
    {
        Path = path;
        IsAdminRequired = isAdminRequired;
        ExecutionTime = DateTime.Now;
    }
}