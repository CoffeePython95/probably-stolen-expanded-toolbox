# Probably Stolen mod template

Standalone starter workspace for a new **Probably Stolen** mod.

## Contents

```text
BasicMod.csproj        minimal netstandard2.1 mod project
BasicSettings.cs       ModConfig settings example
ModEntry.cs            IMod entry point and lifecycle example
manifest.xml           local/Workshop manifest
build.ps1              build and local-install helper
MODDING.md             game paths, APIs, data map, debugging, publishing
docs\decompiled\       decompiled game source and reference mods
reference\Outliner\    full Outliner source as a working example
```

The `reference/Outliner` copy is for reading and copying patterns; it is not
compiled by `BasicMod.csproj`.

## Rename the starter

Replace these consistently:

- `BasicMod` in the project file, namespace, and assembly name.
- `basic_mod` in `manifest.xml` and `ModEntry.ModId`.
- `BasicMod.dll` in `manifest.xml`.

The display name, author, description, and version are in `manifest.xml`.

## Build and install

From this folder:

```powershell
.\build.ps1 -GameDir "E:\SteamLibrary\steamapps\common\Probably Stolen" -Install
```

Then enable and trust the mod in the game's **Mods** screen, and restart the
game. Pass `-GameDir` if the game is installed elsewhere.

## Read next

[`MODDING.md`](MODDING.md) covers:

- where the game, assemblies, local mods, configs, logs, and assets live
- the manifest and loader contract
- `IMod`, `ModHook`, Harmony, `ModConfig`, and asset/localization APIs
- where item/customer/machine data lives in the game code
- build, trust, debugging, and Workshop publishing

Useful starting points in the reference source:

- `reference\Outliner\ModEntry.cs` — lifecycle
- `reference\Outliner\ModSettings.cs` — settings
- `reference\Outliner\Patches.cs` — Harmony examples
- `reference\Outliner\OutlineController.cs` — outline lifecycle
- `reference\Outliner\SearchPanel.cs` / `SettingsUI.cs` — native UI examples
