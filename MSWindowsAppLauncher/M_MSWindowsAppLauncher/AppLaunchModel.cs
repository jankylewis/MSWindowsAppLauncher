
namespace MSWindowsAppLauncher.M_MSWindowsAppLauncher;

internal class AppLaunchModel
{
    internal string Path { get; set; }
    internal bool IsAdminRequired { get; set; }
    internal DateTime ExecutionTime { get; set; }

    internal AppLaunchModel(string in_path, bool in_isAdminRequired)
    {
        Path = in_path;
        IsAdminRequired = in_isAdminRequired;
        ExecutionTime = DateTime.Now;
    }
}