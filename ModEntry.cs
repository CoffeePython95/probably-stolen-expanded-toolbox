using System;

namespace ToolboxExtraRow;

/// <summary>
/// Adds a fourth row to the vanilla toolbox and binds the new slots to
/// 0, - and =.
/// </summary>
public sealed class ModEntry : IMod
{
    internal const string ModId = "toolbox_extra_row";

    internal static bool Enabled { get; private set; }

    internal static ModLog Log { get; private set; }

    public void Init(ModManifest manifest)
    {
        Log = new ModLog(manifest);

        try
        {
            ToolboxSlots.RegisterKeyBindings();
        }
        catch (Exception ex)
        {
            Log.Error("Could not register the extra toolbox keybinds: " + ex);
        }
    }

    public void OnEnable()
    {
        if (Enabled)
        {
            return;
        }

        Enabled = true;
        Log.Log("ToolboxExtraRow enabled; the toolbox now has 12 slots (keys 1-9, 0, - and =).");
    }

    public void OnDisable()
    {
        if (!Enabled)
        {
            return;
        }

        Enabled = false;
        Log.Log("ToolboxExtraRow disabled. Restart the game to restore the vanilla toolbox.");
    }
}
