using System.Collections.Generic;
using ChestSystem.Chest.Data;
using ChestSystem.Chest.UI;
using ChestSystem.Chest.Utilities;
using ChestSystem.Events;
using ChestSystem.UI.Components;
using ChestSystem.UI.Core;
using ChestSystem.UI.Pools;
using UnityEngine;

namespace ChestSystem.Chest.Core
{
    public class ChestService
    {
        private List<ChestScriptableObject> chests;
        private ChestPool chestPool;
        private EmptySlotPool emptySlotPool;
        private List<ChestView> activeChests = new List<ChestView>();
        private List<EmptySlotView> activeEmptySlots = new List<EmptySlotView>();
        private int maxChestSlots;
        private Transform chestParent;
        private ChestView currentlyUnlockingChest = null;

        public List<ChestView> ActiveChests => activeChests;
        public List<EmptySlotView> ActiveEmptySlots => activeEmptySlots;
        public int MaxChestSlots => maxChestSlots;
        public ChestView CurrentlyUnlockingChest => currentlyUnlockingChest;

        public ChestService(List<ChestScriptableObject> chests, ChestView chestPrefab, EmptySlotView emptySlotPrefab, Transform chestParent, int initialMaxChestSlots)
        {
            this.chests = chests;
            this.chestParent = chestParent;
            this.maxChestSlots = initialMaxChestSlots;

            chestPool = new ChestPool(chestPrefab, chestParent);
            emptySlotPool = new EmptySlotPool(emptySlotPrefab, chestParent);

            CreateInitialEmptySlots(initialMaxChestSlots);
        }

        private void CreateInitialEmptySlots(int amount)
        {
            for (int i = 0; i < amount; i++)
                CreateEmptySlot();
        }

        private void AddNewEmptySlots(int amount)
        {
            for (int i = 0; i < amount; i++)
                CreateEmptySlot();
        }

        private void CreateEmptySlot()
        {
            EmptySlotView emptySlot = emptySlotPool.GetEmptySlot();
            emptySlot.gameObject.SetActive(true);
            emptySlot.Initialize();

            int lastIndex = chestParent.childCount - 1;
            emptySlot.transform.SetSiblingIndex(lastIndex);
            AddEmptySlot(emptySlot);
        }

        public void RemoveChestAndMaintainMinimumSlots(ChestView chest, int minimumSlotsToMaintain = 4)
        {
            int siblingIndex = chest.transform.GetSiblingIndex();
            RemoveChest(chest);

            int totalSlots = activeChests.Count + activeEmptySlots.Count;

            if (totalSlots < minimumSlotsToMaintain)
            {
                EmptySlotView emptySlot = emptySlotPool.GetEmptySlot();
                emptySlot.gameObject.SetActive(true);
                emptySlot.Initialize();
                emptySlot.transform.SetSiblingIndex(siblingIndex);
                AddEmptySlot(emptySlot);
            }
            else
            {
                foreach (var activeChest in activeChests)
                {
                    if (activeChest.transform.GetSiblingIndex() > siblingIndex)
                        activeChest.transform.SetSiblingIndex(activeChest.transform.GetSiblingIndex() - 1);
                }

                foreach (var emptySlot in activeEmptySlots)
                {
                    if (emptySlot.transform.GetSiblingIndex() > siblingIndex)
                        emptySlot.transform.SetSiblingIndex(emptySlot.transform.GetSiblingIndex() - 1);
                }
            }
        }

        private bool GetAndRemoveEmptySlot(out int siblingIndex)
        {
            siblingIndex = -1;
            if (ActiveEmptySlots.Count == 0)
                return false;

            EmptySlotView slot = ActiveEmptySlots[0];
            siblingIndex = slot.transform.GetSiblingIndex();
            RemoveEmptySlot(slot);
            emptySlotPool.ReturnEmptySlotToPool(slot);
            return true;
        }

        public void IncreaseMaxChestSlots(int amountToIncrease)
        {
            maxChestSlots += amountToIncrease;
            AddNewEmptySlots(amountToIncrease);
            EventService.Instance.OnMaxSlotsIncreased.InvokeEvent(maxChestSlots);
        }

        public void GenerateRandomChest()
        {
            if (activeChests.Count >= maxChestSlots)
            {
                NotificationManager.Instance.ShowNotification("CHEST SLOTS FULL", "All chest slots are currently full. Add more slots or open existing chests first.", "CLOSE");
                return;
            }

            if (chests == null || chests.Count == 0) return;

            int totalChestGenerationChance = 0;
            foreach (var chestItem in chests)
                totalChestGenerationChance += chestItem.chestGenerationChance;

            int randomValue = Random.Range(0, totalChestGenerationChance);
            foreach (var chestItem in chests)
            {
                if (randomValue < chestItem.chestGenerationChance)
                {
                    SpawnChest(chestItem);
                    return;
                }
                randomValue -= chestItem.chestGenerationChance;
            }
        }

        private void SpawnChest(ChestScriptableObject chestData)
        {
            int siblingIndex;

            if (GetAndRemoveEmptySlot(out siblingIndex))
            {
                ChestView chest = chestPool.GetChest();
                chest.gameObject.SetActive(true);
                chest.Initialize(chestData);

                chest.transform.SetSiblingIndex(siblingIndex);
                AddChest(chest);
            }
        }

        public bool CanStartUnlocking() => currentlyUnlockingChest == null;

        public void SetUnlockingChest(ChestView chest)
        {
            currentlyUnlockingChest = chest;
            EventService.Instance.OnChestUnlockStarted.InvokeEvent(chest);
        }

        public void OnChestUnlockCompleted(ChestView chest)
        {
            if (currentlyUnlockingChest == chest)
            {
                currentlyUnlockingChest = null;
                EventService.Instance.OnChestUnlockCompleted.InvokeEvent(chest);
            }
        }

        public void AddChest(ChestView chest)
        {
            activeChests.Add(chest);
            EventService.Instance.OnChestSpawned.InvokeEvent(chest);
        }

        public void RemoveChest(ChestView chest)
        {
            activeChests.Remove(chest);
            chestPool.ReturnChestToPool(chest);
        }

        public void AddEmptySlot(EmptySlotView slot) => activeEmptySlots.Add(slot);
        public void RemoveEmptySlot(EmptySlotView slot) => activeEmptySlots.Remove(slot);
    }
}