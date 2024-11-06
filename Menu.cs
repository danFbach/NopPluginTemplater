using System.Text.RegularExpressions;

namespace NopPluginTemplater;

public static partial class Menu
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
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You can't be a nameless developer.");
            Console.ForegroundColor = ConsoleColor.White;
            SetDevName();
        }
        else if (devName.EqualsReservedWords(out var matchedWords))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Your dev name equals reserved word(s) {matchedWords}.");
            Console.WriteLine("This will cause problems and must be changed.");
            Console.ForegroundColor = ConsoleColor.White;
            SetDevName();
        }
        else if (!NamespaceRegex().IsMatch(devName))
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Your name contains Illegal characters or formatting for a namespace.");
            Console.WriteLine("This is will likely cause errors and/or warnings.");
            Console.WriteLine("Would you like to change your name? (y/n)");
            Console.ForegroundColor = ConsoleColor.White; 

            if (YesOrNo())
                SetDevName();
            else
                PluginSettings.DevName = devName;
        }
        else if (devName.ContainsReservedWords(out matchedWords))
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Your dev name contains reserved word(s) {matchedWords}.");
            Console.WriteLine("This is will likely cause errors and/or warnings.");
            Console.WriteLine("Would you like to change your name? (y/n)");
            Console.ForegroundColor = ConsoleColor.White;

            if (YesOrNo())
                SetDevName();
            else
                PluginSettings.DevName = devName;
        }
        else
        {
            PluginSettings.DevName = devName;
        }
    }

    private static void SetNopVersion()
    {
        Console.Write("What version of nop are you targeting?");
        Console.WriteLine();
        foreach (var val in Enum.GetValues<NopVersions>())
            Console.WriteLine($"{(int)val}) {nopRawVersion(val)}");

        PluginSettings.NopVersion = getNopVersion();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"nop {nopRawVersion(PluginSettings.NopVersion)} selected.");
        Console.ForegroundColor = ConsoleColor.White;

        static NopVersions getNopVersion()
        {
            var enumVals = Enum.GetValues<NopVersions>();
            var key = Console.ReadKey(true);
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
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"{pluginHumanReadable(PluginSettings.PType)} selected.");
        Console.ForegroundColor = ConsoleColor.White;

        static PluginType getPluginType()
        {
            var enumVals = Enum.GetValues<PluginType>();
            var key = Console.ReadKey(true);
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

        if (string.IsNullOrEmpty(pluginName))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Plugin must be named.");
            Console.ForegroundColor = ConsoleColor.White;
            SetPluginName();
        }
        else if (pluginName.EqualsReservedWords(out var matchedWords))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Your plugin name equals reserved word(s) {matchedWords}.");
            Console.WriteLine("This will cause problems and must be changed.");
            Console.ForegroundColor = ConsoleColor.White;
            SetPluginName();
        }
        else if (!NamespaceRegex().IsMatch(pluginName))
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Your plugin name contains Illegal characters or formatting for a namespace.");
            Console.WriteLine("This is will likely cause errors and/or warnings.");
            Console.WriteLine("Would you like to change the plugin name? (y/n)");
            Console.ForegroundColor = ConsoleColor.White;

            if (YesOrNo())
                SetPluginName();
            else
                PluginSettings.Name = pluginName;
        }
        else if (pluginName.ContainsReservedWords(out matchedWords))
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Your pluing name contains reserved word(s) {matchedWords}.");
            Console.WriteLine("This is will likely cause errors and/or warnings.");
            Console.WriteLine("Would you like to change the plugin name? (y/n)");
            Console.ForegroundColor = ConsoleColor.White;

            if (YesOrNo())
                SetPluginName();
            else
                PluginSettings.Name = pluginName;
        }
        else
        {
            PluginSettings.Name = pluginName;
        }
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

    [GeneratedRegex("(?:[A-Z][a-zA-Z0-9_]+)+[a-z0-9_]")]
    private static partial Regex NamespaceRegex();

    private static readonly string[] ReservedWords = ["abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked", "class", "const", "continue", "decimal", "default", "delegate", "do", "double", "else", "enum", "event", "explicit", "extern", "false", "finally", "fixed", "float", "for", "foreach", "goto", "if", "implicit", "in", "int", "interface", "internal", "is", "lock", "long", "namespace", "new", "null", "object", "operator", "out", "override", "params", "private", "protected", "public", "readonly", "ref", "return", "sbyte", "sealed", "short", "sizeof", "stackalloc", "static", "string", "struct", "switch", "this", "throw", "true", "try", "typeof", "uint", "ulong", "unchecked", "unsafe", "ushort", "using", "virtual", "void", "volatile", "while"];

    private static bool ContainsReservedWords(this string checkString, out string match)
    {
        match = string.Empty;
        if (!ReservedWords.Any(checkString.Contains))
        {
            match = string.Join(", ", ReservedWords.Where(x => checkString.Contains(x, StringComparison.InvariantCultureIgnoreCase)));
            return false;
        }

        return true;
    }

    private static bool EqualsReservedWords(this string checkString, out string match)
    {
        match = string.Empty;
        if (ReservedWords.Any(x => x.Equals(checkString, StringComparison.CurrentCultureIgnoreCase)))
        {
            match = string.Join(", ", ReservedWords.Where(x => x.Equals(checkString, StringComparison.InvariantCultureIgnoreCase)));
            return true;
        }

        return false;
    }
}