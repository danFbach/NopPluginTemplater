using NopPluginTemplater.Generators;

namespace NopPluginTemplater;

public static class PluginSettings
{
    public static string DevName { get; set; } = string.Empty;

    public static string Name { get; set; } = string.Empty;

    public static string NameCamel => string.Concat(Name[0].ToString().ToLower(), Name.AsSpan(1));

    public static PluginType PType { get; set; }

    public static NopVersions NopVersion { get; set; }

    public static bool IsAdminMenu { get; set; } = false;

    public static bool ContainsWidget { get; set; } = false;

    public static string FullPluginNamespace => $"{DevName}.Plugin.{Group}.{Name}";

    public static string Interfaces => Interface + (ContainsWidget ? ", IWidgetPlugin" : string.Empty) + (IsAdminMenu ? ", IAdminMenuPlugin" : string.Empty);

    public static string SystemName => $"{Group}.{Name}";

    public static bool HasServices => !string.IsNullOrWhiteSpace(PluginFields);

    public static List<string> AdditionalProjectFiles = [];

    public static string Usings
    {
        get
        {
            var additionalUsings = new List<string>();

            if (ContainsWidget)
            {
                additionalUsings.Add("Nop.Services.Cms");
                additionalUsings.Add("Nop.Web.Framework.Infrastructure");
                additionalUsings.Add($"{FullPluginNamespace}.Components");
            }

            if (IsAdminMenu)
                additionalUsings.Add("Nop.Web.Framework.Menu");

            additionalUsings.AddRange(TypeSpecificUsing);

            return string.Join(Environment.NewLine, additionalUsings.Distinct().Select(x => $"using {x};"));
        }
    }

    public static string PluginFields
    {
        get
        {
            return string.Join(Environment.NewLine, (PType switch
            {
                PluginType.DiscountRule => new() { { $"{Name}DiscountService", $"_{NameCamel}DiscountService" } },
                PluginType.ExternalAuthenticationMethod => [],
                PluginType.Misc => [],
                PluginType.MultiFactorAuthentication => [],
                PluginType.Payment => new() { { $"{Name}PaymentService", $"_{NameCamel}PaymentService" } },
                PluginType.PickupProvider => [],
                PluginType.ShippingRateComputationMethod => [],
                PluginType.TaxProvider => [],
                _ => new Dictionary<string, string>()
            }).Select(x => $"private readonly {x.Key} {x.Value};"));
        }
    }

    public static string PluginDI
    {
        get
        {
            return string.Join($",{Environment.NewLine}", (PType switch
            {
                PluginType.DiscountRule => new() { { $"{Name}DiscountService", $"{NameCamel}DiscountService" } },
                PluginType.ExternalAuthenticationMethod => [],
                PluginType.Misc => [],
                PluginType.MultiFactorAuthentication => [],
                PluginType.Payment => new() { { $"{Name}PaymentService", $"{NameCamel}PaymentService" } },
                PluginType.PickupProvider => [],
                PluginType.ShippingRateComputationMethod => [],
                PluginType.TaxProvider => [],
                _ => new Dictionary<string, string>()
            }).Select(x => $"{x.Key} {x.Value}"));
        }
    }

    public static string PluginServiceRegistation
    {
        get
        {
            return string.Join(Environment.NewLine, (PType switch
            {
                PluginType.DiscountRule => new() { { $"_{NameCamel}DiscountService", $"{NameCamel}DiscountService" } },
                PluginType.ExternalAuthenticationMethod => [],
                PluginType.Misc => [],
                PluginType.MultiFactorAuthentication => [],
                PluginType.Payment => new() { { $"_{NameCamel}PaymentService", $"{NameCamel}PaymentService" } },
                PluginType.PickupProvider => [],
                PluginType.ShippingRateComputationMethod => [],
                PluginType.TaxProvider => [],
                _ => new Dictionary<string, string>()
            }).Select(x => $"{x.Key} = {x.Value};"));
        }

    }

    private static IEnumerable<string> TypeSpecificUsing
    {
        get
        {
            return PType switch
            {
                PluginType.DiscountRule => ["Nop.Services.Discounts", $"{FullPluginNamespace}.Services"],
                PluginType.ExternalAuthenticationMethod => ["Nop.Services.Authentication.External", $"{FullPluginNamespace}.Components"],
                PluginType.Misc => ["Nop.Services.Common"],
                PluginType.MultiFactorAuthentication => ["Nop.Services.Authentication.MultiFactor", $"{FullPluginNamespace}.Components"],
                PluginType.Payment => ["Nop.Services.Payments", "Nop.Core.Domain.Orders", "Microsoft.AspNetCore.Http", $"{FullPluginNamespace}.Components", $"{FullPluginNamespace}.Services"],
                PluginType.PickupProvider => ["Nop.Services.Shipping.Pickup", "Nop.Core.Domain.Orders", "Nop.Core.Domain.Common", "Nop.Services.Shipping.Tracking"],
                PluginType.ShippingRateComputationMethod => ["Nop.Services.Shipping", "Nop.Services.Shipping.Tracking"],
                PluginType.TaxProvider => ["Nop.Services.Tax"],
                _ => []
            };
        }
    }

    public static string Group
    {
        get
        {
            if (PType == PluginType.Misc && ContainsWidget)
                return "Widgets";

            return PType switch
            {
                PluginType.Misc => "Misc",
                PluginType.Payment => "Payments",
                PluginType.DiscountRule => "DiscountRules",
                PluginType.ShippingRateComputationMethod => "Shipping",
                PluginType.PickupProvider => "Pickup",
                PluginType.TaxProvider => "Tax",
                PluginType.ExternalAuthenticationMethod => "ExternalAuthenticationMethod",
                PluginType.MultiFactorAuthentication => "MultiFactorAuthentication",
                _ => string.Empty
            };
        }
    }

    public static string Interface
    {
        get
        {
            return PType switch
            {
                PluginType.Misc => "IMiscPlugin",
                PluginType.Payment => "IPaymentMethod",
                PluginType.DiscountRule => "IDiscountRequirementRule",
                PluginType.ShippingRateComputationMethod => "IShippingRateComputationMethod",
                PluginType.PickupProvider => "IPickupPointProvider",
                PluginType.TaxProvider => "ITaxProvider",
                PluginType.ExternalAuthenticationMethod => "IExternalAuthenticationMethod",
                PluginType.MultiFactorAuthentication => "IMultiFactorAuthenticationMethod",
                _ => string.Empty
            };
        }
    }

    public static string DotNetVersion
    {
        get
        {
            return NopVersion switch
            {
                NopVersions.nop450 => "net6.0",
                NopVersions.nop460 => "net7.0",
                NopVersions.nop470 => "net8.0",
                NopVersions.nop480 => "net9.0",
                NopVersions.nop490 => string.Empty, //todo: update on nop 4.9 release
                _ => string.Empty
            };
        }
    }

    public static string NopRawVersion
    {
        get
        {

            return NopVersion switch
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

    public static async Task BuildViewComponentsAsync()
    {
        if (ContainsWidget)
        {
            await new Generate("Components", "TWidgetViewComponent").BuildGenericFileAsync();

            await new Generate("Models", "TWidgetViewModel").BuildGenericFileAsync();

            await new Generate("Views/Components", "TWidget").BuildGenericFileAsync(".cshtml");
        }

        if (PType == PluginType.Payment)
        {
            await new Generate("Components", "PaymentInfoViewComponent").BuildGenericFileAsync();

            await new Generate("Models", "PaymentInfoModel").BuildGenericFileAsync();

            await new Generate("Views/Components", "PaymentInfo").BuildGenericFileAsync(".cshtml");

            await new Generate("Services", "TPaymentService").BuildGenericFileAsync();
        }
        else if (PType == PluginType.ExternalAuthenticationMethod)
        {
            await new Generate("Components", "ExternalAuthenticationViewComponent").BuildGenericFileAsync();

            await new Generate("Models", "ExternalAuthenticationModel").BuildGenericFileAsync();

            await new Generate("Views/Components", "ExternalAuthentication").BuildGenericFileAsync(".cshtml");
        }
        else if (PType == PluginType.MultiFactorAuthentication)
        {
            await new Generate("Components", "MultiFactorPublicViewComponent").BuildGenericFileAsync();

            await new Generate("Models", "MultiFactorPublicModel").BuildGenericFileAsync();

            await new Generate("Views/Components", "MultiFactorPublic").BuildGenericFileAsync(".cshtml");


            await new Generate("Components", "MultiFactorVerificationViewComponent").BuildGenericFileAsync();

            await new Generate("Models", "MultiFactorVerificationModel").BuildGenericFileAsync();

            await new Generate("Views/Components", "MultiFactorVerification").BuildGenericFileAsync(".cshtml");
        }
        else if (PType == PluginType.DiscountRule)
        {
            await new Generate("Services", "TDiscountService").BuildGenericFileAsync();
        }
    }
}

public enum NopVersions
{
    nop450,
    nop460,
    nop470,
    nop480,
    nop490,
}

public enum PluginType
{
    Misc,
    Payment,
    DiscountRule,
    ShippingRateComputationMethod,
    PickupProvider,
    TaxProvider,
    ExternalAuthenticationMethod,
    MultiFactorAuthentication
}
