using ChestSystem.Chest;
using ChestSystem.UI;
using ChestSystem.UI.Pools;
using UnityEngine;

public class ChestSlotManager
{
    private ChestController controller;
    private EmptySlotPool emptySlotPool;
    private Transform chestParent;

    public ChestSlotManager(ChestController controller, EmptySlotPool emptySlotPool, Transform chestParent)
    {
        this.controller = controller;
        this.emptySlotPool = emptySlotPool;
        this.chestParent = chestParent;
    }

    public void CreateInitialEmptySlots(int amount)
    {
        for (int i = 0; i < amount; i++)
            CreateEmptySlot();
    }

    public void AddNewEmptySlots(int amount)
    {
        for (int i = 0; i < amount; i++)
            CreateEmptySlot();
    }

    public void CreateEmptySlot()
    {
        EmptySlotView emptySlot = emptySlotPool.GetEmptySlot();
        emptySlot.gameObject.SetActive(true);
        emptySlot.Initialize();

        int lastIndex = chestParent.childCount - 1;
        emptySlot.transform.SetSiblingIndex(lastIndex);
        controller.AddEmptySlot(emptySlot);

        Debug.Log($"Created empty slot. Total slots: {controller.ActiveEmptySlots.Count}");
    }

    public void ReplaceChestWithEmptySlot(ChestView chest)
    {
        int siblingIndex = chest.transform.GetSiblingIndex();
        controller.RemoveChest(chest);

        EmptySlotView emptySlot = emptySlotPool.GetEmptySlot();
        emptySlot.gameObject.SetActive(true);
        emptySlot.Initialize();
        emptySlot.transform.SetSiblingIndex(siblingIndex);
        controller.AddEmptySlot(emptySlot);
    }

    public bool GetAndRemoveEmptySlot(out int siblingIndex)
    {
        siblingIndex = -1;
        if (controller.ActiveEmptySlots.Count == 0)
            return false;

        EmptySlotView slot = controller.ActiveEmptySlots[0];
        siblingIndex = slot.transform.GetSiblingIndex();
        controller.RemoveEmptySlot(slot);
        emptySlotPool.ReturnEmptySlotToPool(slot);
        return true;
    }
}