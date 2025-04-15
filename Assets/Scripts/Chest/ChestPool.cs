using ChestSystem.Chest.UI;
using ChestSystem.Utilities;
using UnityEngine;

namespace ChestSystem.Chest
{
    public class ChestPool : GenericObjectPool<ChestView>
    {
        private ChestView chestPrefab;
        private Transform chestParent;

        public ChestPool(ChestView chestPrefab, Transform chestParent)
        {
            this.chestPrefab = chestPrefab;
            this.chestParent = chestParent;
        }

        public ChestView GetChest() => GetItem<ChestView>();

        protected override ChestView CreateItem<U>()
        {
            ChestView newChest = Object.Instantiate(chestPrefab, chestParent);
            return newChest;
        }

        public void ReturnChestToPool(ChestView chest)
        {
            chest.OnReturnToPool();
            chest.gameObject.SetActive(false);
            ReturnItem(chest);
        }
    }
}