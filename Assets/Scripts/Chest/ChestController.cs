using System.Collections.Generic;
using ChestSystem.Chest.Data;
using ChestSystem.Events;
using UnityEngine;
using ChestSystem.UI.Pools;
using ChestSystem.UI.Components;
using ChestSystem.Chest.UI;
using ChestSystem.Chest.Utilities;

namespace ChestSystem.Chest
{
    public class ChestController
    {
        // Model references
        private List<ChestScriptableObject> chestTypes;
        private int maxChestSlots;

        // View management
        private List<ChestView> activeChests = new List<ChestView>();
        private List<EmptySlotView> activeEmptySlots = new List<EmptySlotView>();
        private ChestPool chestPool;
        private EmptySlotPool emptySlotPool;

        // State tracking
        private ChestView currentlyUnlockingChest = null;
        private Transform chestParent;

        // Properties
        public List<ChestView> ActiveChests => activeChests;
        public List<EmptySlotView> ActiveEmptySlots => activeEmptySlots;
        public int MaxChestSlots => maxChestSlots;
        public bool HasUnlockingChest => currentlyUnlockingChest != null;
        public ChestView CurrentlyUnlockingChest => currentlyUnlockingChest;

        public ChestController(List<ChestScriptableObject> chestTypes, ChestPool chestPool,
                              EmptySlotPool emptySlotPool, Transform chestParent, int initialMaxChestSlots)
        {
            this.chestTypes = chestTypes;
            this.chestPool = chestPool;
            this.emptySlotPool = emptySlotPool;
            this.chestParent = chestParent;
            this.maxChestSlots = initialMaxChestSlots;

            // Create initial empty slots
            CreateInitialEmptySlots(initialMaxChestSlots);
        }

        // Slot Management
        public void CreateInitialEmptySlots(int amount)
        {
            for (int i = 0; i < amount; i++)
                CreateEmptySlot();
        }

        public void IncreaseMaxChestSlots(int amountToIncrease)
        {
            maxChestSlots += amountToIncrease;
            for (int i = 0; i < amountToIncrease; i++)
                CreateEmptySlot();

            EventService.Instance.OnMaxSlotsIncreased.InvokeEvent(maxChestSlots);
            Debug.Log($"Max chest slots increased to {maxChestSlots}");
        }

        private void CreateEmptySlot()
        {
            EmptySlotView emptySlot = emptySlotPool.GetEmptySlot();
            emptySlot.gameObject.SetActive(true);
            emptySlot.Initialize();

            int lastIndex = chestParent.childCount - 1;
            emptySlot.transform.SetSiblingIndex(lastIndex);
            AddEmptySlot(emptySlot);

            Debug.Log($"Created empty slot. Total slots: {activeEmptySlots.Count}");
        }

        // Chest Generation
        public void GenerateRandomChest()
        {
            if (activeChests.Count >= maxChestSlots)
            {
                Debug.Log("All chest slots are full!");
                return;
            }

            if (chestTypes == null || chestTypes.Count == 0) return;

            // Calculate total chance weight
            int totalChestGenerationChance = 0;
            foreach (var chestType in chestTypes)
                totalChestGenerationChance += chestType.chestGenerationChance;

            // Select a chest based on weighted probability
            int randomValue = Random.Range(0, totalChestGenerationChance);
            foreach (var chestType in chestTypes)
            {
                if (randomValue < chestType.chestGenerationChance)
                {
                    SpawnChest(chestType);
                    return;
                }
                randomValue -= chestType.chestGenerationChance;
            }
        }

        private void SpawnChest(ChestScriptableObject chestData)
        {
            int siblingIndex;

            if (GetAndRemoveEmptySlot(out siblingIndex))
            {
                ChestView chest = chestPool.GetChest();
                chest.gameObject.SetActive(true);
                chest.Initialize(chestData, this);

                chest.transform.SetSiblingIndex(siblingIndex);
                AddChest(chest);

                Debug.Log($"Spawned chest: {chestData.chestType}, Active chests: {activeChests.Count}");
            }
            else
            {
                Debug.LogWarning("No empty slots available!");
            }
        }

        // Chest Unlocking
        public bool CanStartUnlocking() => currentlyUnlockingChest == null;

        public void SetUnlockingChest(ChestView chest)
        {
            currentlyUnlockingChest = chest;
            EventService.Instance.OnChestUnlockStarted.InvokeEvent(chest);
        }

        public void ChestUnlockCompleted(ChestView chest)
        {
            if (currentlyUnlockingChest == chest)
            {
                currentlyUnlockingChest = null;
                EventService.Instance.OnChestUnlockCompleted.InvokeEvent(chest);
            }
        }

        // Chest Collection & Rewards
        public void CollectChest(ChestView chest, out int coinsAwarded, out int gemsAwarded)
        {
            coinsAwarded = 0;
            gemsAwarded = 0;

            if (activeChests.Contains(chest))
            {
                // Calculate rewards based on chest type
                ChestScriptableObject chestData = chest.GetChestData();
                coinsAwarded = Random.Range(chestData.minCoinReward, chestData.maxCoinReward + 1);
                gemsAwarded = Random.Range(chestData.minGemReward, chestData.maxGemReward + 1);

                if (currentlyUnlockingChest == chest)
                    currentlyUnlockingChest = null;

                EventService.Instance.OnChestCollected.InvokeEvent(chest, coinsAwarded, gemsAwarded);

                // Replace chest with empty slot
                ReplaceChestWithEmptySlot(chest);
            }
        }

        // Collection and slot management helpers
        public void ReplaceChestWithEmptySlot(ChestView chest)
        {
            int siblingIndex = chest.transform.GetSiblingIndex();
            RemoveChest(chest);

            EmptySlotView emptySlot = emptySlotPool.GetEmptySlot();
            emptySlot.gameObject.SetActive(true);
            emptySlot.Initialize();
            emptySlot.transform.SetSiblingIndex(siblingIndex);
            AddEmptySlot(emptySlot);
        }

        public bool GetAndRemoveEmptySlot(out int siblingIndex)
        {
            siblingIndex = -1;
            if (activeEmptySlots.Count == 0)
                return false;

            EmptySlotView slot = activeEmptySlots[0];
            siblingIndex = slot.transform.GetSiblingIndex();
            RemoveEmptySlot(slot);
            emptySlotPool.ReturnEmptySlotToPool(slot);
            return true;
        }

        // View tracking methods
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