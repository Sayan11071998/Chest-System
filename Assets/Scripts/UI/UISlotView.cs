using UnityEngine;
using UnityEngine.UI;

namespace ChestSystem.UI
{
    public class UISlotView : MonoBehaviour
    {
        [SerializeField] private Image slotBackground;
        [SerializeField] private Color emptySlotColor = Color.gray;
        [SerializeField] private Color occupiedSlotColor = Color.white;

        private Transform chestParent;
        private bool isOccupied = false;
        private int slotIndex;

        private void Awake()
        {
            chestParent = transform.Find("ChestParent");
            if (chestParent == null)
            {
                chestParent = new GameObject("ChestParent").transform;
                chestParent.SetParent(transform);
                chestParent.localPosition = Vector3.zero;
            }

            UpdateVisuals();
        }

        public void SetSlotIndex(int index) => slotIndex = index;

        public int GetSlotIndex() => slotIndex;

        public Transform GetChestParent() => chestParent;

        public void SetOccupied(bool occupied)
        {
            isOccupied = occupied;
            UpdateVisuals();
        }

        public bool IsOccupied() => isOccupied;

        private void UpdateVisuals()
        {
            if (slotBackground != null)
                slotBackground.color = isOccupied ? occupiedSlotColor : emptySlotColor;
        }
    }
}