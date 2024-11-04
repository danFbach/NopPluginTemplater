namespace NopPluginTemplater;

public static class PluginSettings
{
    public static string HackerName { get; set; } = string.Empty;

    public static string Name { get; set; } = string.Empty;

    public static string NameCamel => string.Concat(Name[0].ToString().ToLower(), Name.AsSpan(1));

    public static PluginType PType { get; set; }

    public static bool IsAdminMenu { get; set; } = false;

    public static bool ContainsWidget { get; set; } = false;

    public static string FullPluginNamespace => $"{HackerName}.Plugin.{Group}.{Name}";

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
