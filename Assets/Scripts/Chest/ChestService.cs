using System.Collections.Generic;
using UnityEngine;

namespace ChestSystem.Chest
{
    public class ChestService
    {
        public ChestController chestController { get; private set; }
        private ChestPool chestPool;

        public ChestService(List<ChestScriptableObject> chests, ChestView chestPrefab, Transform chestParent)
        {
            chestPool = new ChestPool(chestPrefab, chestParent);
            chestController = new ChestController(chests, chestPool);
        }

        public void GenerateRandomChest() => chestController.GenerateRandomChest();
    }
}