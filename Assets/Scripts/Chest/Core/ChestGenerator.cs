using System.Collections.Generic;
using ChestSystem.Chest.Data;
using ChestSystem.Chest.UI;
using ChestSystem.Chest.Utilities;
using UnityEngine;

namespace ChestSystem.Chest.Core
{
    public class ChestGenerator
    {
        private ChestController controller;
        private List<ChestScriptableObject> chests;
        private ChestPool chestPool;

        public ChestGenerator(ChestController controller, List<ChestScriptableObject> chests, ChestPool chestPool)
        {
            this.controller = controller;
            this.chests = chests;
            this.chestPool = chestPool;
        }

        public void GenerateRandomChest()
        {
            if (chests == null || chests.Count == 0) return;

            int totalChestGenerationChance = 0;
            foreach (var chestItem in chests)
                totalChestGenerationChance += chestItem.chestGenerationChance;

            int randomValue = Random.Range(0, totalChestGenerationChance);
            foreach (var chestItem in chests)
            {
                if (randomValue < chestItem.chestGenerationChance)
                {
                    SpawnChest(chestItem);
                    return;
                }
                randomValue -= chestItem.chestGenerationChance;
            }
        }

        private void SpawnChest(ChestScriptableObject chestData)
        {
            int siblingIndex;

            if (controller.SlotManager.GetAndRemoveEmptySlot(out siblingIndex))
            {
                ChestView chest = chestPool.GetChest();
                chest.gameObject.SetActive(true);
                chest.Initialize(chestData);

                chest.transform.SetSiblingIndex(siblingIndex);
                controller.AddChest(chest);

                Debug.Log($"Spawned chest: {chestData.chestType}, Active chests: {controller.ActiveChests.Count}");
            }
            else
            {
                Debug.LogWarning("No empty slots available!");
            }
        }
    }
}