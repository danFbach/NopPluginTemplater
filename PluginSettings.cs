namespace NopPluginTemplater;

public static class PluginSettings
{
    public static string HackerName { get; set; } = string.Empty;

    public static string Name { get; set; } = string.Empty;

    public static PluginType PType { get; set; }

    public static bool IsAdminMenu { get; set; } = false;

    public static bool ContainsWidget { get; set; } = false;

    public static string FullPluginNamespace => $"{HackerName}.Plugin.{Group}.{Name}";

    public static string Interfaces => Interface + (ContainsWidget ? ", IWidgetPlugin" : string.Empty) + (IsAdminMenu ? ", IAdminMenuPlugin" : string.Empty);

    public static string SystemName => $"{Group}.{Name}";

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

            additionalUsings = additionalUsings.Distinct().ToList();

            return string.Join(Environment.NewLine, additionalUsings.Select(x => $"using {x};"));
        }
    }

    private static IEnumerable<string> TypeSpecificUsing
    {
        get
        {
            return PType switch
            {
                PluginType.DiscountRule => ["Nop.Services.Discounts"],
                PluginType.ExternalAuthenticationMethod => ["Nop.Services.Authentication.External"],
                PluginType.Misc => ["Nop.Services.Common"],
                PluginType.MultiFactorAuthentication => ["Nop.Services.Authentication.MultiFactor"],
                PluginType.Payment => ["Nop.Services.Payments", "Nop.Core.Domain.Orders", "Microsoft.AspNetCore.Http", $"{FullPluginNamespace}.Components"],
                PluginType.PickupProvider => ["Nop.Services.Shipping.Pickup"],
                PluginType.ShippingRateComputationMethod => ["Nop.Services.Shipping"],
                PluginType.TaxProvider => ["Nop.Services.Tax"],
                _ => []
            };
        }
    }

    public static string Group
    {
        get
        {
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
