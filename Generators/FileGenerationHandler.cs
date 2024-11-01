namespace NopPluginTemplater.Generators;

public static class FileGenerationHandler
{

    public static async Task RunAsync()
    {
        await GenerateRequiredFilesAsync();

        if (PluginSettings.ContainsWidget)
        {
            await new Generate("Components", "TWidgetViewComponent").CodeAsync();

            await new Generate("Models", "TWidgetViewModel").CodeAsync();

            await new Generate("Views/Components", "TWidget").CodeAsync(".cshtml");
        }

        if (PluginSettings.PType == PluginType.Payment)
        {
            await new Generate("Components", "PaymentInfoViewComponent").CodeAsync();

            await new Generate("Models", "PaymentInfoModel").CodeAsync();

            await new Generate("Views/Components", "PaymentInfo").CodeAsync(".cshtml");            
        }

    }

    private static async Task GenerateRequiredFilesAsync()
    {
        await new Generate(string.Empty, "TProject").ProjectFileAsync();

        await new Generate(string.Empty, "pluginjson").PluginJsonAsync();

        await new Generate(string.Empty, "TDefaults").CodeAsync();

        await new Generate(string.Empty, "TSettings").CodeAsync();

        await new Generate(string.Empty, "TPlugin").CodeAsync();

        await new Generate("Infrastructure", "NopStartup").CodeAsync();

        await new Generate("Infrastructure", "RouteProvider").CodeAsync();

        await new Generate("Controllers", "TController").CodeAsync();

        await new Generate("Models", "ConfigurationModel").CodeAsync();

        await new Generate("Views", "_ViewImports").CodeAsync(".cshtml");

        await new Generate("Views", "Configure").CodeAsync(".cshtml");

        Generate.CopyStaticFiles();
    }
}
