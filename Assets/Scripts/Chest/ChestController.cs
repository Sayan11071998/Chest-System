using System.Collections.Generic;
using UnityEngine;

namespace ChestSystem.Chest
{
    public class ChestController
    {
        private List<ChestScriptableObject> chests;

        public ChestController(List<ChestScriptableObject> chests)
        {
            this.chests = chests;
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
                    Debug.Log($"Generated Chest: {chestItem.chestType}");
                    return;
                }
                randomValue -= chestItem.chestGenerationChance;
            }
        }
    }
}