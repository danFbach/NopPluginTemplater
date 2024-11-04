namespace NopPluginTemplater.Generators;

public class Generate(string path, string filename)
{

    private readonly string Path = path;
    private readonly string InputFile = filename;

    private string InputFilePath => !string.IsNullOrEmpty(Path) ? $"./Templates/{Path}/{InputFile}.txt" : $"./Templates/{InputFile}.txt";

    private string OutputPath => !string.IsNullOrEmpty(Path) ? $"./{PluginSettings.FullPluginNamespace}/{Path}" : $"./{PluginSettings.FullPluginNamespace}";
    private string OutputFile => $"{OutputPath}/{(InputFile.StartsWith('T') ? $"{PluginSettings.Name}{InputFile[1..]}" : InputFile)}";

    public async Task CodeAsync(string extension = ".cs")
    {
        if (!File.Exists(InputFilePath))
            return;

        var templateText = await RPX.BulkReplaceAsync(await File.ReadAllTextAsync(InputFilePath));

        if (!Directory.Exists(OutputPath))
            Directory.CreateDirectory(OutputPath);

        if (!extension.StartsWith('.'))
            extension = $".{extension}";

        if (extension == ".cshtml")
            PluginSettings.AdditionalProjectFiles.Add(OutputFile.Replace($"./{PluginSettings.FullPluginNamespace}/", string.Empty).Replace('/', '\\') + extension);

        await File.WriteAllTextAsync($"{OutputFile}{extension}", templateText);
    }

    public async Task ProjectFileAsync()
    {
        if (!File.Exists(InputFilePath))
            return;

        var templateText = await RPX.BulkReplaceAsync(await File.ReadAllTextAsync(InputFilePath));

        if (!Directory.Exists(OutputPath))
            Directory.CreateDirectory(OutputPath);

        await File.WriteAllTextAsync($"{OutputPath}/{PluginSettings.FullPluginNamespace}.csproj", templateText);
    }

    public async Task PluginJsonAsync()
    {
        if (!File.Exists(InputFilePath))
            return;

        var templateText = await RPX.BulkReplaceAsync(await File.ReadAllTextAsync(InputFilePath));

        if (!Directory.Exists(OutputPath))
            Directory.CreateDirectory(OutputPath);

        await File.WriteAllTextAsync($"{OutputPath}/plugin.json", templateText);
    }

    public static void CopyStaticFiles()
    {
        if (!Directory.Exists($"./{PluginSettings.FullPluginNamespace}/temp"))
            Directory.CreateDirectory($"./{PluginSettings.FullPluginNamespace}/temp");

        File.Copy("./Templates/temp/readme.txt", $"./{PluginSettings.FullPluginNamespace}/temp/readme.txt", true);
    }
}
