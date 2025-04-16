using UnityEngine;

namespace ChestSystem.Chest
{
    [CreateAssetMenu(fileName = "ChestScriptableObject", menuName = "Chest/NewChest")]
    public class ChestScriptableObject : ScriptableObject
    {
        [Header("Chest Specifications")]
        public ChestType chestType;
        public Sprite chestSprite;
        public Sprite unlockedChestSprite;
        [Tooltip("Percentage chance this chest will spawn (0-100)")]
        [Range(0, 100)]
        public int chestGenerationChance;

        [Header("Awarding Specifications")]
        public float unlockTimeInSeconds;
        public int instantOpenCostInGems;

        [Header("Reward Section")]
        public int minCoinReward;
        public int maxCoinReward;
        public int minGemReward;
        public int maxGemReward;

        public void CalculateRewards(out int coinsAwarded, out int gemsAwarded)
        {
            coinsAwarded = Random.Range(minCoinReward, maxCoinReward + 1);
            gemsAwarded = Random.Range(minGemReward, maxGemReward + 1);
        }
    }
}