namespace NopPluginTemplater;

public static class Menu
{
    public static void MakeSelections()
    {
        SetDevName();

        SetNopVersion();

        SetPluginType();

        SetWidgetPreference();

        SetAdminMenuPerference();

        SetPluginName();
    }

    private static void SetDevName()
    {
        Console.Write("Enter your developer name: ");

        var devName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(devName))
            throw new Exception("You can't be a nameless developer.");

        //TODO: validation for namespace req's
        PluginSettings.DevName = devName;

        Console.WriteLine();
        Console.WriteLine();
    }

    private static void SetNopVersion()
    {
        Console.Write("What version of nop are you targeting?");
        Console.WriteLine();
        foreach (var val in Enum.GetValues<NopVersions>())
            Console.WriteLine($"{(int)val}) {nopRawVersion(val)}");

        PluginSettings.NopVersion = getNopVersion();

        static NopVersions getNopVersion()
        {
            var enumVals = Enum.GetValues<NopVersions>();
            var key = Console.ReadKey();
            if (int.TryParse(key.KeyChar.ToString(), out var keyInt) &&
                keyInt < enumVals.Length)
            {
                return (NopVersions)keyInt;
            }

            return getNopVersion();
        }
        static string nopRawVersion(NopVersions nopVersion)
        {
            return nopVersion switch
            {
                NopVersions.nop450 => "4.50",
                NopVersions.nop460 => "4.60",
                NopVersions.nop470 => "4.70",
                NopVersions.nop480 => "4.80",
                NopVersions.nop490 => "4.90",
                _ => string.Empty
            };
        }
    }

    private static void SetPluginType()
    {
        Console.Write("What type of plugin do want to make?");
        Console.WriteLine();
        foreach (var val in Enum.GetValues<PluginType>())
            Console.WriteLine($"{(int)val}) {pluginHumanReadable(val)}");

        PluginSettings.PType = getPluginType();

        static PluginType getPluginType()
        {
            var enumVals = Enum.GetValues<PluginType>();
            var key = Console.ReadKey();
            if (int.TryParse(key.KeyChar.ToString(), out var keyInt) &&
                keyInt < enumVals.Length)
            {
                return (PluginType)keyInt;
            }

            return getPluginType();
        }

        static string pluginHumanReadable(PluginType pType)
        {
            return pType switch
            {
                PluginType.Misc => "Misc",
                PluginType.Payment => "Payment",
                PluginType.DiscountRule => "Discount Rule",
                PluginType.ShippingRateComputationMethod => "Shipping Method",
                PluginType.PickupProvider => "Pickup Provider",
                PluginType.TaxProvider => "Tax Provider",
                PluginType.ExternalAuthenticationMethod => "External Authentication Method",
                PluginType.MultiFactorAuthentication => "Multi-Factor Authentication Method",
                _ => string.Empty
            };
        }
    }

    private static void SetWidgetPreference()
    {
        Console.WriteLine();
        Console.WriteLine("Will this plugin contain a widget? (y/n)");

        PluginSettings.ContainsWidget = YesOrNo();
    }

    private static void SetAdminMenuPerference()
    {
        Console.WriteLine();
        Console.WriteLine("Should this plugin be visible on the admin menu? (y/n)");

        PluginSettings.IsAdminMenu = YesOrNo();
    }

    private static void SetPluginName()
    {
        Console.WriteLine();
        Console.WriteLine($"Current namespace: {PluginSettings.FullPluginNamespace}{{PluginName}}");

        Console.WriteLine();
        Console.Write("Name your plugin: ");
        var pluginName = Console.ReadLine();

        //TODO: validation for namespace req's
        if (string.IsNullOrEmpty(pluginName))
            throw new Exception("Plugin must be named.");

        PluginSettings.Name = pluginName;
    }

    private static bool YesOrNo()
    {
        bool? result = null;

        while (!result.HasValue)
        {
            var entry = Console.ReadKey(true);
            if (entry.Key == ConsoleKey.Y)
            {
                result = true;
            }
            else if (entry.Key == ConsoleKey.N)
            {
                result = false;
            }
        }

        return result.Value;
    }
}