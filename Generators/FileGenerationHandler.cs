namespace NopPluginTemplater.Generators;

public static class FileGenerationHandler
{

    public static async Task RunAsync()
    {
        await GenerateRequiredFilesAsync();

        await PluginSettings.BuildViewComponentsAsync();

        await new Generate(string.Empty, "TProject").BuildProjectFileAsync();
    }

    private static async Task GenerateRequiredFilesAsync()
    {
        await new Generate(string.Empty, "pluginjson").BuildPluginJsonAsync();

        await new Generate(string.Empty, "TDefaults").BuildGenericFileAsync();

        await new Generate(string.Empty, "TSettings").BuildGenericFileAsync();

        await new Generate(string.Empty, "TPlugin").BuildGenericFileAsync();

        await new Generate("Infrastructure", "NopStartup").BuildGenericFileAsync();

        await new Generate("Infrastructure", "RouteProvider").BuildGenericFileAsync();

        await new Generate("Controllers", "TController").BuildGenericFileAsync();

        await new Generate("Models", "ConfigurationModel").BuildGenericFileAsync();

        await new Generate("Views", "_ViewImports").BuildGenericFileAsync(".cshtml");

        await new Generate("Views", "Configure").BuildGenericFileAsync(".cshtml");

        Generate.CopyStaticFiles();
    }
}
