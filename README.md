# Expanded Toolbox

A **Probably Stolen** mod that gives the toolbox a fourth row of three slots.

## Features

- The original nine slots keep their keys: `1`-`9`.
- The new slots 10, 11 and 12 are bound to `0`, `-` and `=` by default.
  They can be rebound in the game's keybind options like any other action.
- The three new slots start locked and are unlocked one at a time with scrap
  metal, exactly like the vanilla toolbox slots.
- Existing saves keep their unlocked slot count and simply gain the three new
  locked slots at the bottom.
- The toolbox tooltip and the "fully upgraded" sprite now use 12 slots.

## Install (players)

No build tools are needed.

1. Go to the [latest release](../../releases/latest).
2. Download `ExpandedToolbox-v<version>.zip`.
3. Extract the `ExpandedToolbox` folder from the zip into:

   ```text
   <Steam library>\steamapps\common\Probably Stolen\Mods\
   ```

   The result should look like:

   ```text
   Probably Stolen\Mods\ExpandedToolbox\manifest.xml
   Probably Stolen\Mods\ExpandedToolbox\ExpandedToolbox.dll
   ```

4. Start the game, open **Mods**, trust/enable **Expanded Toolbox**, and restart.

The mod targets **Probably Stolen 0.46D**. Since it changes item creation,
disabling it requires a restart.

## Build from source

Requirements:

- Probably Stolen installed locally.
- .NET 8 SDK.

From this folder:

```powershell
.\build.ps1 -GameDir "E:\SteamLibrary\steamapps\common\Probably Stolen"
```

This produces:

```text
dist\ExpandedToolbox\                  unpacked mod folder
dist\ExpandedToolbox-v0.1.0.zip       release archive
```

To build, package and install directly into the local game:

```powershell
.\build.ps1 -GameDir "E:\SteamLibrary\steamapps\common\Probably Stolen" -Install
```

Then enable/trust the mod in the game's **Mods** screen and restart. Pass
`-GameDir` if the game is installed elsewhere.

The build uses the game's own `Assembly-CSharp.dll`, so it cannot be compiled
on a machine without the game installed. The release ZIP is built locally and
uploaded to GitHub Releases.

## Releasing

See [`RELEASING.md`](RELEASING.md) for the exact source and GitHub release flow.

## Implementation notes

The mod uses Harmony patches on `ToolboxHelper`:

- `CreateToolboxWindow` builds a 12-slot, 3-column window instead of the
  vanilla 9-slot one.
- `GetUnlockedSlots` / `CanUpgradeToolbox` allow the unlock tag to grow from
  9 to 12 while still reading old save values unchanged.
- `CreateToolboxTooltip`, `UpdateToolboxSprite` and `HandleHotkeys` use the
  expanded slot count and hotkey range.
- `ToolboxHelper.Bindings` gets three extra `toolbox_slot_N` entries, so the
  new keys appear in the normal keybind settings and persist through the
  game's existing keybind save system.

The extra panels are appended after the vanilla nine. The save system stores
inventory nodes by traversal index, so old saves continue to map items to the
same first nine slots; the new slots load empty.

See `MODDING.md` for general Probably Stolen modding information.
