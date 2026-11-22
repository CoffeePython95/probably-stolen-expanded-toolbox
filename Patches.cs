using System;
using System.Collections.Generic;
using HarmonyLib;

namespace ToolboxExtraRow;

[HarmonyPatch(typeof(ToolboxHelper), nameof(ToolboxHelper.CreateToolboxWindow))]
internal static class CreateToolboxWindowPatch
{
    [HarmonyPrefix]
    internal static bool Prefix(ref ValueTuple<PixelWindow, List<GameSlotInventory>, List<MachineryLabel>> __result)
    {
        __result = ToolboxSlots.CreateWindow();
        return false;
    }
}

[HarmonyPatch(typeof(ToolboxHelper), nameof(ToolboxHelper.GetUnlockedSlots))]
internal static class GetUnlockedSlotsPatch
{
    [HarmonyPostfix]
    internal static void Postfix(GameItem box, ref int __result)
    {
        // The vanilla method clamps to 9. Read the saved tag directly so old
        // saves with 3-9 unlocked slots keep their value and can grow to 12.
        __result = Math.Clamp(
            box.GetTagReadonly(ToolboxHelper.TOOLBOX_UNLOCKED_SLOTS_INT).GetInt(),
            ToolboxHelper.TOOLBOX_START_SLOTS,
            ToolboxSlots.TotalSlots);
    }
}

[HarmonyPatch(typeof(ToolboxHelper), nameof(ToolboxHelper.CanUpgradeToolbox))]
internal static class CanUpgradeToolboxPatch
{
    [HarmonyPostfix]
    internal static void Postfix(GameItem box, ref bool __result)
    {
        __result = box.IsTag(ToolboxHelper.TOOLBOX_TAG)
            && ToolboxHelper.GetUnlockedSlots(box) < ToolboxSlots.TotalSlots;
    }
}

[HarmonyPatch(typeof(ToolboxHelper), "UpdateToolboxSprite")]
internal static class UpdateToolboxSpritePatch
{
    [HarmonyPrefix]
    internal static bool Prefix(GameItem box)
    {
        bool fullyUnlocked = ToolboxHelper.GetUnlockedSlots(box) >= ToolboxSlots.TotalSlots;
        box.SetSpriteAndShape("Items/items_tool2", fullyUnlocked ? "toolbox_new" : "toolbox_old");
        return false;
    }
}

[HarmonyPatch(typeof(ToolboxHelper), nameof(ToolboxHelper.CreateToolboxTooltip))]
internal static class CreateToolboxTooltipPatch
{
    [HarmonyPrefix]
    internal static bool Prefix(RichTextBuilder builder, GameItem item)
    {
        if (!item.IsTag(ToolboxHelper.TOOLBOX_TAG))
        {
            return false;
        }

        int unlockedSlots = ToolboxHelper.GetUnlockedSlots(item);
        builder.AddLine(
            LocHelper.GetLocalizedMechanic("mech_toolbox_tooltip_slots", unlockedSlots, ToolboxSlots.TotalSlots),
            ignoreEmpty: true,
            RenderHandler.ColorPalette.LightCyan);

        if (unlockedSlots < ToolboxSlots.TotalSlots)
        {
            builder.AddLine(
                LocHelper.GetLocalizedMechanic("mech_toolbox_tooltip_upgrade"),
                ignoreEmpty: true,
                RenderHandler.ColorPalette.LightGray);
        }

        builder.AddEmptyLine(14);
        return false;
    }
}

[HarmonyPatch(typeof(ToolboxHelper), nameof(ToolboxHelper.HandleHotkeys))]
internal static class HandleToolboxHotkeysPatch
{
    [HarmonyPrefix]
    internal static bool Prefix()
    {
        if (DialogUIManager.Instance != null && DialogUIManager.Instance.IsChoice())
        {
            return false;
        }

        for (int i = 0; i < ToolboxSlots.TotalSlots; i++)
        {
            if (KeyBindHelper.GetKeyDown(KeyBindHelper.ToolboxSlot(i)))
            {
                ToolboxHelper.TryToggleEquipSlot(i);
                break;
            }
        }

        return false;
    }
}
