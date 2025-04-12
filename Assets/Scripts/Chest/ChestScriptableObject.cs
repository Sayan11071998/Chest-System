using UnityEngine;

namespace ChestSystem.Chest
{
    [CreateAssetMenu(fileName = "ChestScriptableObject", menuName = "Chest/NewChest")]
    public class ChestScriptableObject : ScriptableObject
    {
        [Header("Chest Specifications")]
        public ChestType chestType;
        public Sprite chestSprite;
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
    }
}