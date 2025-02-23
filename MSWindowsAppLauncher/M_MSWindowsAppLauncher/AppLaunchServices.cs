
using System.Diagnostics;
using Microsoft.CSharp.RuntimeBinder;

namespace MSWindowsAppLauncher.M_MSWindowsAppLauncher;

internal class AppLaunchServices
{
    private AppLaunchServices() 
        => throw new RuntimeBinderException("Util classes must not be instantiated!     ");

    internal static async Task S_LaunchApps(List<AppLaunchModel> in_appLaunchModels)
    {
        List<Task> tasks = new();

        foreach (AppLaunchModel? l_appLaunchModel in in_appLaunchModels)
        {
            tasks.Add(Task.Run(() =>
            {
                try
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = l_appLaunchModel.Path,
                        UseShellExecute = true,
                        Verb = l_appLaunchModel.IsAdminRequired ? "runas" : ""
                    };

                    Process.Start(startInfo);
                    l_appLaunchModel.ExecutionTime = DateTime.Now;
                    Console.WriteLine($"[{l_appLaunchModel.ExecutionTime:HH:mm:ss}]_ CMD EXECUTED -> " +
                                      $"{l_appLaunchModel.Path}{(l_appLaunchModel.IsAdminRequired ? " -admin" : "")}");
                }
                catch (Exception a_ex)
                {
                    Console.WriteLine($"Error launching {l_appLaunchModel.Path}: {a_ex.Message}     ");
                }
            }));
        }

        await Task.WhenAll(tasks);
    }
    
    internal static async Task S_KillExistingInstances()
    {
        try
        {
            int currentPid = Process.GetCurrentProcess().Id;
                
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "taskkill",
                Arguments = $"/F /FI \"PID ne {currentPid}\" /IM MSWindowsAppLauncher.exe",
                UseShellExecute = true,
                Verb = "runas", // This requests admin privileges
                CreateNoWindow = true
            };

            Process? process = Process.Start(startInfo);
            
            if (process != null)
                await process.WaitForExitAsync();
        }
        catch (Exception a_ex)
        {
            Console.WriteLine($"Warning: Could not kill existing instances: {a_ex.Message}      ");
        }
    }
}