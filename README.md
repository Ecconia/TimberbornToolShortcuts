# Mod ToolShortcuts for Timberborn

Adds the ability to assign keybindings to vanilla tool groups in your toolbar and keybindings to switch between tools.

Default config (no collision with vanilla keybindings):
- `Shift + G`: Open Tree Cutting tools
- `Shift + F`: Open Field Planting tools
- `Shift + T`: Open Forestry Planting tools
- `X`: Open Demolishing tools
- `Shift + 1, Shift + 2, ..., Shift + 9, Shift + 0`: Select tool

![Demo](https://github.com/zallek/TimberbornToolShortcuts/raw/main/demo.gif)

## Requirements

- [Harmony](https://mod.io/g/timberborn/m/harmony)

## Installation

- Decompress the zip archive in Timberborns mod folder `C:/users/<user>/Documents/Timberborn/Mods/`.

## Configuration

- You can assign/change keybindings easily via the vanilla keybindings settings.
- In the main settings, you can choose if the first tool should automatically be selected when opening a tool group by keybinding.

## Building

To build the mod, you have to export an environment variable to the game installation directory before building.

On Linux that looks something like this:
```bash
export Timberborn="/home/ecconia/.local/share/Steam/steamapps/common/Timberborn/"
dotnet run --project ResourceGenerator
dotnet build -c Release
```
