using System.Collections.Generic;
using UnityEngine;

namespace ChestSystem.Chest
{
    public class ChestSlotController
    {
        private List<ChestSlot> chestSlots;
        private int maxSlots = 4;

        public ChestSlotController(int maxSlotCount)
        {
            maxSlots = maxSlotCount;

            InitializeSlots();
        }

        private void InitializeSlots()
        {
            chestSlots = new List<ChestSlot>(maxSlots);

            for (int i = 0; i < maxSlots; i++)
                chestSlots.Add(new ChestSlot());
        }

        public bool HasEmptySlots()
        {
            foreach (var slot in chestSlots)
            {
                if (!slot.IsOccupied)
                    return true;
            }

            return false;
        }

        public ChestSlot GetEmptySlot()
        {
            foreach (var slot in chestSlots)
            {
                if (!slot.IsOccupied)
                    return slot;
            }

            return null;
        }

        public ChestModel GetUnlockingChest()
        {
            foreach (var slot in chestSlots)
            {
                if (slot.IsOccupied && slot.currentChestModel.CurrentState == ChestState.UNLOCKING)
                    return slot.currentChestModel;
            }

            return null;
        }

        public bool HasUnlockingChest() => GetUnlockingChest() != null;
        public List<ChestSlot> GetAllSlots() => chestSlots;

        public void RemoveChest(ChestSlot slot)
        {
            int index = chestSlots.IndexOf(slot);

            if (index >= 0)
                chestSlots[index].ClearSlot();
        }
    }
}