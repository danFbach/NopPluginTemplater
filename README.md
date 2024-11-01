# nop Plugin Templater

**this is a work in progress**

a simple (command line, for now) tool to instantly build out a scaffolded version of a plugin, based on plugin type and needs
* no more copy/paste plugin building
* changes namespaces automatically
* no unused code from previous plugin
* no forgetting to change the output path 😅
* all Interface methods and properties implemented immediately, with default `throw new NotImplementedException()`
* all necessary usings injected
