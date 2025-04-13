using System.Collections.Generic;
using ChestSystem.Slot;
using ChestSystem.UI;
using UnityEngine;

namespace ChestSystem.Chest
{
    public class ChestService
    {
        public ChestController chestController { get; private set; }
        private ChestPool chestPool;
        private EmptySlotPool emptySlotPool;

        public ChestService(List<ChestScriptableObject> chests, ChestView chestPrefab, EmptySlotView emptySlotPrefab, Transform chestParent, int initialMaxChestSlots)
        {
            chestPool = new ChestPool(chestPrefab, chestParent);
            emptySlotPool = new EmptySlotPool(emptySlotPrefab, chestParent);
            chestController = new ChestController(chests, chestPool, emptySlotPool, chestParent, initialMaxChestSlots);
        }

        public void GenerateRandomChest() => chestController.GenerateRandomChest();
        public void IncreaseMaxChestSlots(int amountToIncrease) => chestController.IncreaseMaxChestSlots(amountToIncrease);
        public void CollectChest(ChestView chest, out int coinsAwarded, out int gemsAwarded) =>
                            chestController.CollectChest(chest, out coinsAwarded, out gemsAwarded);
    }
}