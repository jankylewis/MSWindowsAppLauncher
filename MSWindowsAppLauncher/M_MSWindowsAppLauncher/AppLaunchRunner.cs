
namespace MSWindowsAppLauncher.M_MSWindowsAppLauncher;

internal class AppLaunchRunner
{
    internal static async Task S_ExecuteLaunchingApps()
    {
        // Kill any existing instances first
        await AppLaunchServices.S_KillExistingInstances();

        // Define the default file path
        string basePath = Path.Combine(Directory.GetCurrentDirectory(), "_apps_to_be_executed");
        string filePath = Path.Combine(basePath, "_app_commands_01.txt");

        // Create directory if it doesn't exist
        if (!Directory.Exists(basePath))
        {
            Directory.CreateDirectory(basePath);

            Console.WriteLine("\n");

            Console.WriteLine($"Created directory: <{basePath}>     ");
            Console.WriteLine($"Please create <{filePath}> with your application paths!       ");
            Console.WriteLine("Format example:");
            Console.WriteLine("C:\\Program Files\\App1\\App1.exe");
            Console.WriteLine("C:\\Program Files\\App2\\App2.exe -admin");

            Console.WriteLine("\n");

            Environment.Exit(0);
        }

        // Check if file exists
        if (!File.Exists(filePath))
        {
            Console.WriteLine("\n");

            Console.WriteLine($"Error: File not found at <{filePath}>     ");
            Console.WriteLine("Please create the file with your application paths!      ");
            Console.WriteLine("Format example:      ");
            Console.WriteLine("C:\\Program Files\\App1\\App1.exe    ");
            Console.WriteLine("C:\\Program Files\\App2\\App2.exe -admin     ");

            Console.WriteLine("\n");

            Environment.Exit(1);
        }

        List<AppLaunchModel> apps = new();

        // Read and parse the text file
        try
        {
            string[] lines = File.ReadAllLines(filePath);

            foreach (string l_line in lines)
            {
                if (string.IsNullOrWhiteSpace(l_line))
                    continue;

                bool isAdminRequired = false;
                string path = l_line.Trim();

                // Check if admin flag is present
                if (path.EndsWith(" -admin", StringComparison.OrdinalIgnoreCase))
                {
                    isAdminRequired = true;
                    
                    // Remove the " -admin" from the path
                    path = path.Substring(0, path.Length - 7).Trim(); 
                }

                // Validate if file exists
                if (!File.Exists(path))
                {
                    Console.WriteLine($"Warning: File not found at {path}       ");
                    continue;
                }

                // Validate if file is an .exe
                if (!path.ToLower().EndsWith(".exe"))
                {
                    Console.WriteLine($"Warning: File {path} is not an executable (.exe)        ");
                    continue;
                }

                apps.Add(new AppLaunchModel(path, isAdminRequired));
            }
        }
        catch (Exception a_ex)
        {
            Console.WriteLine($"Error reading file: {a_ex.Message}      ");
            Environment.Exit(1);
        }

        if (apps.Count is 0)
        {
            Console.WriteLine("No valid applications found in the file.     ");
            Environment.Exit(1);
        }

        try
        {
            await AppLaunchServices.S_LaunchApps(apps);
            Environment.Exit(0);
        }
        catch (Exception a_ex)
        {
            Console.WriteLine($"Error launching applications: {a_ex.Message}        ");
            Environment.Exit(1);
        }
    }
}

