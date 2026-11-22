using System.Collections.Generic;
using UnityEngine;

namespace ToolboxExtraRow;

/// <summary>
/// Shared constants and UI construction for the expanded toolbox.
/// </summary>
internal static class ToolboxSlots
{
    internal const int VanillaSlots = 9;
    internal const int ExtraSlots = 3;
    internal const int TotalSlots = VanillaSlots + ExtraSlots;

    private static readonly KeyCode[] ExtraKeys =
    {
        KeyCode.Alpha0,
        KeyCode.Minus,
        KeyCode.Equals,
    };

    private static bool bindingsRegistered;

    /// <summary>
    /// Vanilla registers keybinds for slots 1-9 only. The extra slots reuse the
    /// same "toolbox_slot_N" ids so labels, rebinding and persistence all go
    /// through the game's normal keybind system.
    /// </summary>
    internal static void RegisterKeyBindings()
    {
        if (bindingsRegistered)
        {
            return;
        }

        bindingsRegistered = true;

        for (int extra = 0; extra < ExtraKeys.Length; extra++)
        {
            int slotIndex = VanillaSlots + extra;
            int slotNumber = slotIndex + 1;
            string id = KeyBindHelper.ToolboxSlot(slotIndex);

            if (KeyBindHelper.Find(id) != null)
            {
                continue;
            }

            KeyBindHelper.Bindings.Add(new KeyBindHelper.Binding
            {
                id = id,
                defaultKey = ExtraKeys[extra],
                label = () => LocHelper.GetLocalizedMainMenu("option_key_toolbox_slot", slotNumber),
            });
        }
    }

    /// <summary>
    /// Mirrors ToolboxHelper.CreateToolboxWindow but creates 12 slots instead
    /// of 9. The three extra panels are appended, so save files that reference
    /// the original nine inventory nodes keep the same indices.
    /// </summary>
    internal static (PixelWindow, List<GameSlotInventory>, List<MachineryLabel>) CreateWindow()
    {
        List<GameSlotInventory> slots = new List<GameSlotInventory>(TotalSlots);
        List<MachineryLabel> labels = new List<MachineryLabel>(TotalSlots);
        List<MachineryPanel> panels = new List<MachineryPanel>(TotalSlots);

        for (int i = 0; i < TotalSlots; i++)
        {
            GameSlotInventory slot = new GameSlotInventory(2, 2);
            slot.SetBackgroundFadeSprite("Items/items_tool", "screwdriver_s");

            MachineryPanel panel = MachineHelper.Panel(SlotLabel(i), slot, "slot" + i);
            slots.Add(slot);
            labels.Add(panel.label);
            panels.Add(panel);
        }

        PixelWindow window = MachineHelper.CreateMachineryWindow(canMove: true, panels.ToArray());
        ((MachineryPanelRow)window.childElement).SetColumns(3);
        window.SetTitle(LocHelper.GetLocalizedItem("item_toolbox_name"));
        window.Validate();
        return (window, slots, labels);
    }

    private static string SlotLabel(int slotIndex)
    {
        string label = LocHelper.GetLocalizedMechanic("mech_toolbox_label_slot", slotIndex + 1);
        string id = KeyBindHelper.ToolboxSlot(slotIndex);
        KeyCode key = KeyBindHelper.Get(id);

        if (key == KeyBindHelper.GetDefault(id))
        {
            return label;
        }

        return label + " (" + KeyBindHelper.KeyName(key) + ")";
    }
}
