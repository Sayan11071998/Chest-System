using System.Collections.Generic;
using UnityEngine;

namespace ChestSystem.Chest.Utilities
{
    public class ChestRewardCalculator
    {
        private List<ChestScriptableObject> chests;

        public ChestRewardCalculator(List<ChestScriptableObject> chests)
        {
            this.chests = chests;
        }

        public void CalculateRewards(ChestView chest, out int coinsAwarded, out int gemsAwarded)
        {
            coinsAwarded = 0;
            gemsAwarded = 0;

            ChestScriptableObject chestData = GetChestData(chest);
            if (chestData != null)
            {
                coinsAwarded = Random.Range(chestData.minCoinReward, chestData.maxCoinReward + 1);
                gemsAwarded = Random.Range(chestData.minGemReward, chestData.maxGemReward + 1);
            }
        }

        private ChestScriptableObject GetChestData(ChestView chest)
        {
            foreach (var chestData in chests)
            {
                if (chestData.chestType.ToString() == chest.name)
                    return chestData;
            }
            return null;
        }
    }
}