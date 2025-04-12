using UnityEngine;
using ChestSystem.Chest;

#if UNITY_EDITOR
using UnityEditor;

namespace ChestSystem.Editor
{
    public class ChestScriptableObjectsSetup
    {
        [MenuItem("ChestSystem/Create Default Chest Data")]
        public static void CreateDefaultChestData()
        {
            CreateCommonChestData();
            CreateRareChestData();
            CreateEpicChestData();
            CreateLegendaryChestData();

            Debug.Log("Created default chest data!");
        }

        private static void CreateCommonChestData()
        {
            ChestScriptableObject asset = ScriptableObject.CreateInstance<ChestScriptableObject>();

            asset.chestType = ChestType.COMMON;
            asset.chestGenerationChance = 50;
            asset.unlockTimeInSeconds = 1800;
            asset.instantOpenCostInGems = 3;
            asset.minCoinReward = 10;
            asset.maxCoinReward = 50;
            asset.minGemReward = 1;
            asset.maxGemReward = 3;

            AssetDatabase.CreateAsset(asset, "Assets/ScriptableObjects/Chests/CommonChest.asset");
        }

        private static void CreateRareChestData()
        {
            ChestScriptableObject asset = ScriptableObject.CreateInstance<ChestScriptableObject>();

            asset.chestType = ChestType.RARE;
            asset.chestGenerationChance = 30;
            asset.unlockTimeInSeconds = 10800;
            asset.instantOpenCostInGems = 10;
            asset.minCoinReward = 50;
            asset.maxCoinReward = 150;
            asset.minGemReward = 3;
            asset.maxGemReward = 8;

            AssetDatabase.CreateAsset(asset, "Assets/ScriptableObjects/Chests/RareChest.asset");
        }

        private static void CreateEpicChestData()
        {
            ChestScriptableObject asset = ScriptableObject.CreateInstance<ChestScriptableObject>();

            asset.chestType = ChestType.EPIC;
            asset.chestGenerationChance = 15;
            asset.unlockTimeInSeconds = 28800;
            asset.instantOpenCostInGems = 20;
            asset.minCoinReward = 150;
            asset.maxCoinReward = 300;
            asset.minGemReward = 8;
            asset.maxGemReward = 15;

            AssetDatabase.CreateAsset(asset, "Assets/ScriptableObjects/Chests/EpicChest.asset");
        }

        private static void CreateLegendaryChestData()
        {
            ChestScriptableObject asset = ScriptableObject.CreateInstance<ChestScriptableObject>();

            asset.chestType = ChestType.LEGENDARY;
            asset.chestGenerationChance = 5;
            asset.unlockTimeInSeconds = 86400;
            asset.instantOpenCostInGems = 50;
            asset.minCoinReward = 300;
            asset.maxCoinReward = 600;
            asset.minGemReward = 15;
            asset.maxGemReward = 30;

            AssetDatabase.CreateAsset(asset, "Assets/ScriptableObjects/Chests/LegendaryChest.asset");
        }
    }
}
#endif