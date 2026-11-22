namespace BasicMod;

/// <summary>
/// Add settings through ModConfig. The game builds the Mods-screen rows from
/// these definitions automatically.
/// </summary>
internal static class BasicSettings
{
    private static ConfigEntry<bool> enabled;

    internal static void Initialize()
    {
        ModConfig config = ModHelper.GetConfig(ModEntry.ModId);
        enabled = config.Define(
            "enabled",
            true,
            "General",
            () => "Enabled",
            () => "Turn this mod's feature on or off.");
    }

    internal static bool Enabled
    {
        get { return enabled == null || enabled.Value; }
    }
}
