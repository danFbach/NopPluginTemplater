namespace NopPluginTemplater;

public static class RPX
{
    public static async Task<string> BulkReplaceAsync(string templateContents)
    {
        templateContents = templateContents
            .Replace("{namespace}", PluginSettings.FullPluginNamespace)
            .Replace("{SystemName}", PluginSettings.SystemName)
            .Replace("{PluginName}", PluginSettings.Name)
            .Replace("{PluginTypes}", PluginSettings.Interfaces)
            .Replace("{PluginNameCamel}", string.Concat(PluginSettings.Name[0].ToString().ToLower(), PluginSettings.Name.AsSpan(1)))
            .Replace("{PluginUsings}", PluginSettings.Usings)
            .Replace("{PluginGroup}", PluginSettings.Group);

        if (templateContents.Contains("{PluginImplementation}"))
        {
            var implementation = await File.ReadAllTextAsync($"./Templates/PluginImplementations/{PluginSettings.Interface}.txt");

            templateContents = templateContents.Replace("{PluginImplementation}", implementation);
        }

        if(PluginSettings.ContainsWidget &&
            templateContents.Contains("{WidgetImplementation}"))
        {
            var implementation = await File.ReadAllTextAsync($"./Templates/PluginImplementations/IWidgetPlugin.txt");

            implementation = await BulkReplaceAsync(implementation);

            templateContents = templateContents.Replace("{WidgetImplementation}", implementation);
        }

        return templateContents;
    }
}
