using NopPluginTemplater;
using NopPluginTemplater.Generators;

Menu.MakeSelections();

await FileGenerationHandler.RunAsync();