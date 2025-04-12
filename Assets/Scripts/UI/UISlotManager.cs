using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ChestSystem.Chest;

namespace ChestSystem.UI
{
    public class UISlotManager : MonoBehaviour
    {
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private Transform slotsParent;
        [SerializeField] private int defaultSlotCount = 4;
        [SerializeField] private ScrollRect scrollRect;

        private List<UISlotView> slotViews = new List<UISlotView>();
        private ChestController chestController;

        public void Initialize(ChestController controller, int slotCount = -1)
        {
            chestController = controller;
            if (slotCount <= 0) slotCount = defaultSlotCount;

            CreateSlots(slotCount);
        }

        private void CreateSlots(int count)
        {
            foreach (Transform child in slotsParent)
                Destroy(child.gameObject);

            slotViews.Clear();

            for (int i = 0; i < count; i++)
            {
                GameObject slotObject = Instantiate(slotPrefab, slotsParent);
                UISlotView slotView = slotObject.GetComponent<UISlotView>();

                if (slotView != null)
                {
                    slotView.SetSlotIndex(i);
                    slotViews.Add(slotView);
                }
            }
        }

        public void UpdateSlotsFromController()
        {
            var allSlots = chestController.GetAllSlots();

            while (slotViews.Count < allSlots.Count)
            {
                GameObject slotObject = Instantiate(slotPrefab, slotsParent);
                UISlotView slotView = slotObject.GetComponent<UISlotView>();

                if (slotView != null)
                {
                    slotView.SetSlotIndex(slotViews.Count);
                    slotViews.Add(slotView);
                }
            }

            for (int i = 0; i < allSlots.Count; i++)
            {
                if (i < slotViews.Count)
                {
                    slotViews[i].SetOccupied(allSlots[i].IsOccupied);

                    if (allSlots[i].IsOccupied && allSlots[i].currentChestView != null)
                    {
                        Transform chestTransform = allSlots[i].currentChestView.transform;
                        Transform slotChestParent = slotViews[i].GetChestParent();

                        if (chestTransform.parent != slotChestParent)
                        {
                            chestTransform.SetParent(slotChestParent);
                            chestTransform.localPosition = Vector3.zero;
                            chestTransform.localScale = Vector3.one;
                        }
                    }
                }
            }
        }

        public void ScrollToSlot(int slotIndex)
        {
            if (scrollRect != null && slotIndex >= 0 && slotIndex < slotViews.Count)
            {
                Canvas.ForceUpdateCanvases();

                float normalizedPosition = (float)slotIndex / (slotViews.Count - 1);

                if (scrollRect.vertical)
                    scrollRect.verticalNormalizedPosition = 1 - normalizedPosition;
                else if (scrollRect.horizontal)
                    scrollRect.horizontalNormalizedPosition = normalizedPosition;
            }
        }
    }
}