using UnityEngine;

namespace ChestSystem.Chest
{
    [CreateAssetMenu(fileName = "ChestScriptableObject", menuName = "Chest/NewChest")]
    public class ChestScriptableObject : ScriptableObject
    {
        public ChestType chestType;
        public int chestGenerationChance;
    }
}