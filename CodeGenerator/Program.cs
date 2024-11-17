
/// <summary>
/// Replaces placeholders in the template with the provided inputs.
/// </summary>
static string ReplacePlaceholders(string template, string namespaceName, string modelName)
{
    string modelNameLower = char.ToLower(modelName[0]) + modelName.Substring(1);

    return template
        .Replace("{Namespace}", namespaceName)
        .Replace("{ModelName}", modelName)
        .Replace("{modelName}", modelNameLower);
}


/// <summary>
/// Reads the template, replaces placeholders, and writes the output file.
/// </summary>
static async Task GenerateFileAsync(string templatePath, string outputPath, string namespaceName, string modelName)
{
    try
    {
        if (!File.Exists(templatePath))
        {
            Console.WriteLine($"Template file not found: {templatePath}");
            return;
        }

        string template = await File.ReadAllTextAsync(templatePath);
        string content = ReplacePlaceholders(template, namespaceName, modelName);

        string outputDirectory = Path.GetDirectoryName(outputPath);
        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        await File.WriteAllTextAsync(outputPath, content);
        Console.WriteLine($"File generated successfully: {outputPath}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while generating the file: {ex.Message}");
    }
}


Console.WriteLine("Welcome to Code Generator!");

Console.WriteLine("Enter the directory path where the generated files should be saved:");
string directoryPath = Console.ReadLine()!.Trim();

Console.WriteLine("Enter the model name (e.g., Customer):");
string modelName = Console.ReadLine()!.Trim();

Console.WriteLine("Enter the namespace for the controller (Press Enter to use default: BaseClassLib.Infologs.Controller):");
string controllerNamespace = Console.ReadLine()!.Trim();
if (string.IsNullOrEmpty(controllerNamespace))
{
    controllerNamespace = "BaseClassLib.Infologs.Controller";
}

Console.WriteLine("Enter the namespace for the repository (Press Enter to use default: BaseClassLib.Infologs.Repository):");
string repositoryNamespace = Console.ReadLine()!.Trim();
if (string.IsNullOrEmpty(repositoryNamespace))
{
    repositoryNamespace = "BaseClassLib.Infologs.Repository";
}

// Define template paths
string templatesDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates");
string repositoryTemplatePath = Path.Combine(templatesDirectory, "RepositoryTemplate.txt");
string controllerTemplatePath = Path.Combine(templatesDirectory, "ControllerTemplate.txt");

// Define output file paths
string repositoryOutputPath = Path.Combine(directoryPath, $"{modelName}Repository.cs");
string controllerOutputPath = Path.Combine(directoryPath, $"{modelName}Controller.cs");

// Generate Repository and Controller
Console.WriteLine("\nGenerating Repository...");
await GenerateFileAsync(repositoryTemplatePath, repositoryOutputPath, repositoryNamespace, modelName);

Console.WriteLine("\nGenerating Controller...");
await GenerateFileAsync(controllerTemplatePath, controllerOutputPath, controllerNamespace, modelName);

Console.WriteLine("\nCode generation completed!");
Console.WriteLine($"Check your output directory: {directoryPath}");