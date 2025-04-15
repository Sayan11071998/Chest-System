using System.Collections.Generic;
using ChestSystem.Chest.Data;
using ChestSystem.Chest.UI;
using ChestSystem.Chest.Core;
using ChestSystem.UI.Components;
using ChestSystem.UI.Pools;
using UnityEngine;

namespace ChestSystem.Chest.Core
{
    public class ChestService
    {
        private ChestController controller;

        public ChestService(List<ChestScriptableObject> chests, ChestView chestPrefab,
                           EmptySlotView emptySlotPrefab, Transform chestParent, int initialMaxChestSlots)
        {
            ChestPool chestPool = new ChestPool(chestPrefab, chestParent);
            EmptySlotPool emptySlotPool = new EmptySlotPool(emptySlotPrefab, chestParent);
            controller = new ChestController(chests, chestPool, emptySlotPool, chestParent, initialMaxChestSlots);
        }

        // Public API methods
        public void GenerateRandomChest() => controller.GenerateRandomChest();

        public void IncreaseMaxChestSlots(int amountToIncrease) => controller.IncreaseMaxChestSlots(amountToIncrease);

        public void CollectChest(ChestView chest, out int coinsAwarded, out int gemsAwarded) =>
            controller.CollectChest(chest, out coinsAwarded, out gemsAwarded);

        public void SetUnlockingChest(ChestView chest) => controller.SetUnlockingChest(chest);

        public void ChestUnlockCompleted(ChestView chest) => controller.ChestUnlockCompleted(chest);

        public void ClearUnlockingChest(ChestView chest)
        {
            if (controller.CurrentlyUnlockingChest == chest)
                controller.ChestUnlockCompleted(chest);
        }

        public bool CanStartUnlocking() => controller.CanStartUnlocking();
    }
}