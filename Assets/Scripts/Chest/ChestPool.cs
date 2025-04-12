using System.Collections.Generic;
using ChestSystem.Utilities;
using UnityEngine;

namespace ChestSystem.Chest
{
    public class ChestPool : GenericObjectPool<ChestView>
    {
        private Dictionary<ChestType, GameObject> chestPrefabs;
        private Transform chestParent;

        public ChestPool(Dictionary<ChestType, GameObject> prefabs, Transform parent)
        {
            chestPrefabs = prefabs;
            chestParent = parent;
        }

        public ChestView GetChestView(ChestType chestType) => GetItem<ChestView>();

        protected override ChestView CreateItem<U>()
        {
            GameObject prefab = null;

            foreach (var pair in chestPrefabs)
            {
                prefab = pair.Value;
                break;
            }

            if (prefab == null)
            {
                Debug.LogError("No chest prefabs assigned to ChestPool!");
                return null;
            }

            GameObject chestObject = GameObject.Instantiate(prefab, chestParent);
            return chestObject.GetComponent<ChestView>();
        }
    }
}