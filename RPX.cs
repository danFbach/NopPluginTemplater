namespace NopPluginTemplater;

public static class RPX
{
    public static async Task<string> BulkReplaceAsync(string templateContents)
    {
        foreach (var rpl in ReplaceActions)
        {
            if (templateContents.Contains(rpl.Key))
                templateContents = await rpl.Value.func.Invoke(rpl.Key, templateContents, rpl.Value.replace);
        }

        return templateContents;
    }

    private static readonly IReadOnlyDictionary<string, (string replace, Func<string, string, string, Task<string>> func)> ReplaceActions =
        new Dictionary<string, (string replace, Func<string, string, string, Task<string>> func)>()
    {
        { "{namespace}", (PluginSettings.FullPluginNamespace, GenericReplace) },
        { "{SystemName}", (PluginSettings.SystemName, GenericReplace) },
        { "{PluginName}", (PluginSettings.Name, GenericReplace) },
        { "{PluginTypes}", (PluginSettings.Interfaces, GenericReplace) },
        { "{PluginNameCamel}", (PluginSettings.NameCamel, GenericReplace) },
        { "{PluginUsings}", (PluginSettings.Usings, GenericReplace) },
        { "{PluginGroup}", (PluginSettings.Group, GenericReplace) },
        { "{NopRawVersion}", (PluginSettings.NopRawVersion, GenericReplace) },
        { "{DotNetVersion}", (PluginSettings.DotNetVersion, GenericReplace) },
        { "{AdditionalProjectFiles}", (string.Empty, AdditionalProjectFiles) },
        { "{ConfigurePluginService}", (string.Empty, ConfigureServices) },
        { "{WidgetImplementation}", (string.Empty, WidgetImplementation) },
        { "{PluginImplementation}", (string.Empty, PluginImplementation) },
        { "{PluginFields}", (string.Empty, PluginFields) },
    };

    private static Task<string> GenericReplace(string key, string contents, string replaceContent)
    {
        return Task.FromResult(contents.Replace(key, replaceContent));
    }

    private static async Task<string> WidgetImplementation(string key, string contents, string _)
    {
        var implementation = string.Empty;

        if (PluginSettings.ContainsWidget)
        {
            implementation = await File.ReadAllTextAsync($"./Templates/PluginImplementations/IWidgetPlugin.txt");

            implementation = await BulkReplaceAsync(implementation);
        }

        return contents.Replace(key, implementation);
    }

    private static async Task<string> PluginImplementation(string key, string contents, string _)
    {
        var implementation = await File.ReadAllTextAsync($"./Templates/PluginImplementations/{PluginSettings.Interface}.txt");
        return contents.Replace(key, implementation);
    }

    private static Task<string> PluginFields(string key, string contents, string _)
    {
        return Task.FromResult(contents.Replace("{PluginFields}", PluginSettings.PluginFields)
            .Replace("{PluginServiceDI}", !string.IsNullOrWhiteSpace(PluginSettings.PluginDI) ? $",{Environment.NewLine}{PluginSettings.PluginDI}" : string.Empty)
            .Replace("{PluginServiceRegistation}", PluginSettings.PluginServiceRegistation));
    }

    private static Task<string> ConfigureServices(string key, string contents, string _)
    {
        var serviceName = getServiceName();

        contents = contents.Replace(key, !string.IsNullOrEmpty(serviceName) ? $"services.AddScoped<{serviceName}>();" : string.Empty);
        contents = contents.Replace("{PluginServiceUsings}", !string.IsNullOrEmpty(serviceName) ? $"using {PluginSettings.FullPluginNamespace}.Services;" : string.Empty);

        return Task.FromResult(contents);

        static string getServiceName()
        {
            return PluginSettings.PType switch
            {
                PluginType.Payment => $"{PluginSettings.Name}PaymentService",
                PluginType.DiscountRule => $"{PluginSettings.Name}DiscountService",
                _ => string.Empty
            };
        }
    }

    private static Task<string> AdditionalProjectFiles(string key, string contents, string _)
    {
        var projectFiles = string.Join(Environment.NewLine, PluginSettings.AdditionalProjectFiles.Select(x =>
        $"    <Content Include=\"{x}\">\r\n" +
        $"      <CopyToOutputDirectory>Always</CopyToOutputDirectory>\r\n" +
        $"    </Content>"));

        return Task.FromResult(contents.Replace(key, projectFiles));
    }
}
