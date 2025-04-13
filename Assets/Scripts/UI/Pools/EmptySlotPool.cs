using ChestSystem.Utilities;
using UnityEngine;

namespace ChestSystem.UI.Pools
{
    public class EmptySlotPool : GenericObjectPool<EmptySlotView>
    {
        private EmptySlotView emptySlotPrefab;
        private Transform slotParent;

        public EmptySlotPool(EmptySlotView emptySlotPrefab, Transform slotParent)
        {
            this.emptySlotPrefab = emptySlotPrefab;
            this.slotParent = slotParent;
        }

        public EmptySlotView GetEmptySlot() => GetItem<EmptySlotView>();

        protected override EmptySlotView CreateItem<U>()
        {
            EmptySlotView newEmptySlot = Object.Instantiate(emptySlotPrefab, slotParent);
            return newEmptySlot;
        }

        public void ReturnEmptySlotToPool(EmptySlotView emptySlot)
        {
            emptySlot.gameObject.SetActive(false);
            ReturnItem(emptySlot);
        }
    }
}