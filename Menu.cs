using Microsoft.VisualBasic;

namespace NopPluginTemplater;

public class Menu
{

    public static void MakeSelections()
    {

        Console.Write("Enter your hacker handle: ");

        var hackerName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(hackerName))
            throw new Exception("You can't be a nameless hacker.");

        //TODO validation for namespace requirements
        PluginSettings.HackerName = hackerName;

        Console.WriteLine();
        Console.WriteLine();

        Console.Write("What version of nop are you targeting?");
        Console.WriteLine();
        foreach (var val in Enum.GetValues<NopVersions>())
            Console.WriteLine($"{(int)val}) {NopRawVersion(val)}");

        PluginSettings.NopVersion = GetNopVersion();

        Console.Write("What type of plugin do want to make?");
        Console.WriteLine();
        foreach (var val in Enum.GetValues<PluginType>())
            Console.WriteLine($"{(int)val}) {PluginHR(val)}");

        PluginSettings.PType = GetPluginType();

        Console.WriteLine();
        Console.WriteLine("Will this plugin contain a widget? (y/n)");


        PluginSettings.ContainsWidget = YesOrNo();

        Console.WriteLine();
        Console.WriteLine("Should this plugin be visible on the admin menu? (y/n)");

        PluginSettings.IsAdminMenu = YesOrNo();

        Console.WriteLine();
        Console.WriteLine($"Current namespace: {PluginSettings.FullPluginNamespace}{{PluginName}}");

        Console.WriteLine();
        Console.Write("Name your plugin: ");
        var pluginName = Console.ReadLine();

        if (string.IsNullOrEmpty(pluginName))
            throw new Exception("Plugin must be named.");

        PluginSettings.Name = pluginName;

    }

    private static string PluginHR(PluginType pType)
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

    public static string NopRawVersion(NopVersions nopVersion)
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

    private static PluginType GetPluginType()
    {
        var enumVals = Enum.GetValues<PluginType>();
        var key = Console.ReadKey();
        if (int.TryParse(key.KeyChar.ToString(), out var keyInt) &&
            keyInt < enumVals.Length)
        {
            return (PluginType)keyInt;
        }

        return GetPluginType();
    }

    private static NopVersions GetNopVersion()
    {
        var enumVals = Enum.GetValues<NopVersions>();
        var key = Console.ReadKey();
        if (int.TryParse(key.KeyChar.ToString(), out var keyInt) &&
            keyInt < enumVals.Length)
        {
            return (NopVersions)keyInt;
        }

        return GetNopVersion();
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