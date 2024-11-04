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

            await new Generate("Services", "TPaymentService").CodeAsync();
        }

        if (PluginSettings.PType == PluginType.ExternalAuthenticationMethod)
        {
            await new Generate("Components", "ExternalAuthenticationViewComponent").CodeAsync();

            await new Generate("Models", "ExternalAuthenticationModel").CodeAsync();

            await new Generate("Views/Components", "ExternalAuthentication").CodeAsync(".cshtml");
        }

        if (PluginSettings.PType == PluginType.MultiFactorAuthentication)
        {
            await new Generate("Components", "MultiFactorPublicViewComponent").CodeAsync();

            await new Generate("Models", "MultiFactorPublicModel").CodeAsync();

            await new Generate("Views/Components", "MultiFactorPublic").CodeAsync(".cshtml");


            await new Generate("Components", "MultiFactorVerificationViewComponent").CodeAsync();

            await new Generate("Models", "MultiFactorVerificationModel").CodeAsync();

            await new Generate("Views/Components", "MultiFactorVerification").CodeAsync(".cshtml");
        }

        if (PluginSettings.PType == PluginType.DiscountRule)
        {
            await new Generate("Services", "TDiscountService").CodeAsync();
        }

        await new Generate(string.Empty, "TProject").ProjectFileAsync();
    }

    private static async Task GenerateRequiredFilesAsync()
    {
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
