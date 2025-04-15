using UnityEngine;

namespace ChestSystem.Chest
{
    public class ChestModel
    {
        public ChestType chestType { get; private set; }
        public Sprite chestLockedSprite { get; private set; }
        public Sprite chestUnlockedSprite { get; private set; }
        public int chestGenerationChance { get; private set; }
        public float unlockTimeInSeconds { get; private set; }
        public int minCoinReward { get; private set; }
        public int maxCoinReward { get; private set; }
        public int minGemReward { get; private set; }
        public int maxGemReward { get; private set; }

        public ChestModel(ChestScriptableObject chestData)
        {
            chestType = chestData.chestType;
            chestLockedSprite = chestData.chestLockedSprite;
            chestUnlockedSprite = chestData.chestUnlockedSprite;
            chestGenerationChance = chestData.chestGenerationChance;
            unlockTimeInSeconds = chestData.unlockTimeInSeconds;
            minCoinReward = chestData.minCoinReward;
            maxCoinReward = chestData.maxCoinReward;
            minGemReward = chestData.minGemReward;
            maxGemReward = chestData.maxGemReward;
        }
    }
}