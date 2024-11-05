# nop Plugin Templater

**this is a work in progress**

a simple (command line, for now) tool to instantly build out a scaffolded version of a plugin, based on plugin type and needs
* no more copy/paste plugin building
* changes namespaces automatically
* no unused code from previous plugin
* no forgetting to change the output path 😅
* generates view component resources for widgets and plugins that require components (i.e. Payment, MultiFactorAuth, etc.)
  * EX: `Components/TWidgetViewComponent.cs`, `Models/TWidgetViewModel.cs` and `Views/Components/TWidget.cshtml`
* manipulates .csproj to copy views and other resources when needed.
* all Interface methods and properties implemented with either `throw new NotImplementedException()` or a pregenerated `ViewComponenent` when applicable.
* all necessary usings injected
