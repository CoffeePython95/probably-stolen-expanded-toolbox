using System;

namespace BasicMod;

/// <summary>
/// Minimal built-in Probably Stolen mod entry point.
/// Rename this namespace and the mod ID when starting a real mod.
/// </summary>
public sealed class ModEntry : IMod
{
    internal const string ModId = "basic_mod";

    internal static bool Enabled { get; private set; }

    internal static ModLog Log { get; private set; }

    public void Init(ModManifest manifest)
    {
        Log = new ModLog(manifest);
        BasicSettings.Initialize();
    }

    public void OnEnable()
    {
        if (Enabled)
        {
            return;
        }

        Enabled = true;
        ModHook.OnGameLoadedNormal += OnGameLoaded;
        Log.Log("BasicMod enabled.");
    }

    public void OnDisable()
    {
        Enabled = false;
        ModHook.OnGameLoadedNormal -= OnGameLoaded;
        Log.Log("BasicMod disabled.");
    }

    private static void OnGameLoaded()
    {
        if (!Enabled || !PlayerStore.HasInstance)
        {
            return;
        }

        try
        {
            int count = PlayerStore.Instance.FindAllItem().Count;
            Log.Log("Store loaded; visible item count: " + count);
        }
        catch (Exception ex)
        {
            Log.Warning("Could not inspect the store: " + ex.Message);
        }
    }
}
