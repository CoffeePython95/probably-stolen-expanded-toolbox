# Expanded Toolbox

A **Probably Stolen** mod that gives the toolbox a fourth row of three slots.

- The original nine slots keep their keys: `1`-`9`.
- The new slots 10, 11 and 12 are bound to `0`, `-` and `=` by default.
  They can be rebound in the game's keybind options like any other action.
- The three new slots start locked and are unlocked one at a time with scrap
  metal, exactly like the vanilla toolbox slots.
- Existing saves keep their unlocked slot count and simply gain the three new
  locked slots at the bottom.
- The toolbox tooltip and the "fully upgraded" sprite now use 12 slots.

## Build and install

From this folder:

```powershell
.\build.ps1 -GameDir "E:\SteamLibrary\steamapps\common\Probably Stolen" -Install
```

Then enable/trust **Expanded Toolbox** in the game's **Mods** screen and
restart the game. Pass `-GameDir` if the game is installed elsewhere.

Since the mod changes item creation, disabling it requires a restart.

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
