using System.Collections.Generic;
using ChestSystem.Chest.UI;
using ChestSystem.Chest.Utilities;
using ChestSystem.Events;
using ChestSystem.UI.Components;
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

        // Properties
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

        #region Chest Slot Management

        /// <summary>
        /// Create initial empty slots
        /// </summary>
        private void CreateInitialEmptySlots(int amount)
        {
            for (int i = 0; i < amount; i++)
                CreateEmptySlot();
        }

        /// <summary>
        /// Add new empty slots
        /// </summary>
        private void AddNewEmptySlots(int amount)
        {
            for (int i = 0; i < amount; i++)
                CreateEmptySlot();
        }

        /// <summary>
        /// Create a single empty slot
        /// </summary>
        private void CreateEmptySlot()
        {
            EmptySlotView emptySlot = emptySlotPool.GetEmptySlot();
            emptySlot.gameObject.SetActive(true);
            emptySlot.Initialize();

            int lastIndex = chestParent.childCount - 1;
            emptySlot.transform.SetSiblingIndex(lastIndex);
            AddEmptySlot(emptySlot);

            Debug.Log($"Created empty slot. Total slots: {ActiveEmptySlots.Count}");
        }

        /// <summary>
        /// Replace a chest with an empty slot
        /// </summary>
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

        /// <summary>
        /// Get and remove an empty slot
        /// </summary>
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

        #endregion

        #region Chest System Functions

        /// <summary>
        /// Increase the maximum number of chest slots
        /// </summary>
        public void IncreaseMaxChestSlots(int amountToIncrease)
        {
            maxChestSlots += amountToIncrease;
            AddNewEmptySlots(amountToIncrease);
            EventService.Instance.OnMaxSlotsIncreased.InvokeEvent(maxChestSlots);
            Debug.Log($"Max chest slots increased to {maxChestSlots}");
        }

        /// <summary>
        /// Generate a random chest if there are empty slots
        /// </summary>
        public void GenerateRandomChest()
        {
            if (activeChests.Count >= maxChestSlots)
            {
                Debug.Log("All chest slots are full!");
                return;
            }

            if (chests == null || chests.Count == 0) return;

            // Calculate total generation chance
            int totalChestGenerationChance = 0;
            foreach (var chestItem in chests)
                totalChestGenerationChance += chestItem.chestGenerationChance;

            // Choose a random chest based on weighted probability
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

        /// <summary>
        /// Spawn a specific chest
        /// </summary>
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

                Debug.Log($"Spawned chest: {chestData.chestType}, Active chests: {ActiveChests.Count}");
            }
            else
            {
                Debug.LogWarning("No empty slots available!");
            }
        }

        /// <summary>
        /// Check if a chest can start unlocking
        /// </summary>
        public bool CanStartUnlocking() => currentlyUnlockingChest == null;

        /// <summary>
        /// Set the chest that is currently being unlocked
        /// </summary>
        public void SetUnlockingChest(ChestView chest)
        {
            currentlyUnlockingChest = chest;
            EventService.Instance.OnChestUnlockStarted.InvokeEvent(chest);
        }

        /// <summary>
        /// Called when a chest has finished unlocking
        /// </summary>
        public void OnChestUnlockCompleted(ChestView chest)
        {
            if (currentlyUnlockingChest == chest)
            {
                currentlyUnlockingChest = null;
                EventService.Instance.OnChestUnlockCompleted.InvokeEvent(chest);
            }
        }

        /// <summary>
        /// Add a chest to the active chests list
        /// </summary>
        public void AddChest(ChestView chest)
        {
            activeChests.Add(chest);
            EventService.Instance.OnChestSpawned.InvokeEvent(chest);
        }

        /// <summary>
        /// Remove a chest from the active chests list
        /// </summary>
        public void RemoveChest(ChestView chest)
        {
            activeChests.Remove(chest);
            chestPool.ReturnChestToPool(chest);
        }

        /// <summary>
        /// Add an empty slot to the active empty slots list
        /// </summary>
        public void AddEmptySlot(EmptySlotView slot) => activeEmptySlots.Add(slot);

        /// <summary>
        /// Remove an empty slot from the active empty slots list
        /// </summary>
        public void RemoveEmptySlot(EmptySlotView slot) => activeEmptySlots.Remove(slot);

        #endregion
    }
}