# nop Plugin Templater

**this is a work in progress**

a simple (command line, for now) tool to instantly build out a scaffolded version of a plugin, based on plugin type and needs
* no more copy/paste plugin building
* changes namespaces automatically
* no unused code from previous plugin
* no forgetting to change the output path 😅
* generates view component resources for widgets and plugins that require components (i.e. Payment, MultiFactorAuth, etc.)
  * EX: `Components/TWidgetViewComponent.cs`, `Models/TWidgetViewModel.cs` and `Views/Components/TWidget.cshtml`, where `T` is replaced by the plugin name.
* manipulates .csproj to copy views and other resources when needed.
* all Plugin Interface methods and properties implemented with either pregenerated defaults for `GetConfigurationUrl` and `ViewComponenent`, or `throw new NotImplementedException()` when usage cannot be assumed.
* all necessary usings injected
