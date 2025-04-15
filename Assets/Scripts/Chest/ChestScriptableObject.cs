using UnityEngine;

namespace ChestSystem.Chest
{
    [CreateAssetMenu(fileName = "ChestScriptableObject", menuName = "Chest/NewChest")]
    public class ChestScriptableObject : ScriptableObject
    {
        [Header("Chest Specifications")]
        public ChestType chestType;

        [Header("Chest Sprites")]
        public Sprite chestLockedSprite;
        public Sprite chestUnlockedSprite;

        [Tooltip("Percentage chance this chest will spawn (0-100)")]
        [Range(0, 100)]
        public int chestGenerationChance;

        [Header("Awarding Specifications")]
        public float unlockTimeInSeconds;

        [Header("Reward Section")]
        public int minCoinReward;
        public int maxCoinReward;
        public int minGemReward;
        public int maxGemReward;
    }
}