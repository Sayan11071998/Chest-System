using System;
using System.Collections.Generic;
using ChestSystem.Player;
using UnityEngine;

namespace ChestSystem.Chest
{
    public class ChestController
    {
        private List<ChestScriptableObject> chests;
        private ChestSlotController slotController;
        private ChestPool chestPool;
        private Transform chestSlotsParent;
        private PlayerController playerController;
        private Queue<ChestSlot> unlockQueue = new Queue<ChestSlot>();

        private ChestModel lastSkippedChest;
        private int lastSkippedGemCost;
        private bool canUndo = false;

        public ChestController(List<ChestScriptableObject> chests, Transform slotsParent, Dictionary<ChestType, GameObject> chestPrefabs, PlayerController playerController)
        {
            this.chests = chests;
            this.chestSlotsParent = slotsParent;
            this.playerController = playerController;

            slotController = new ChestSlotController(4);
            chestPool = new ChestPool(chestPrefabs, slotsParent);
        }

        public void Update() => UpdateChestTimers();

        private void UpdateChestTimers()
        {
            var allSlots = slotController.GetAllSlots();

            foreach (var slot in allSlots)
            {
                if (!slot.IsOccupied) continue;

                var chestModel = slot.currentChestModel;

                if (chestModel.CurrentState == ChestState.UNLOCKING)
                {
                    chestModel.RemainingTimeInSeconds -= Time.deltaTime;

                    if (chestModel.RemainingTimeInSeconds <= 0)
                    {
                        chestModel.RemainingTimeInSeconds = 0;
                        chestModel.CurrentState = ChestState.UNLOCKED;
                        Debug.Log($"Chest unlocked: {chestModel.ChestScriptableObject.chestType}");

                        StartNextChestInQueue();
                    }

                    slot.currentChestView.UpdateVisuals();
                }
            }
        }

        public void GenerateRandomChest()
        {
            if (!slotController.HasEmptySlots())
            {
                Debug.LogWarning("No empty slots available!");
                return;
            }

            if (chests == null || chests.Count == 0) return;

            int totalChestGenerationChance = 0;

            foreach (var chestItem in chests)
                totalChestGenerationChance += chestItem.chestGenerationChance;

            int randomValue = UnityEngine.Random.Range(0, totalChestGenerationChance);

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
            var emptySlot = slotController.GetEmptySlot();

            if (emptySlot == null) return;

            var chestModel = new ChestModel(chestData);
            var chestView = chestPool.GetChestView(chestModel.ChestScriptableObject.chestType);

            chestView.SetActive(true);
            chestView.SetParent(chestSlotsParent);
            chestView.Initialize(chestModel, OnChestClicked);

            emptySlot.AssignChest(chestModel, chestView);

            Debug.Log($"Generated Chest: {chestData.chestType}");
        }

        private void OnChestClicked(ChestView chestView)
        {
            var chestModel = chestView.GetChestModel();

            switch (chestModel.CurrentState)
            {
                case ChestState.LOCKED:
                    ShowUnlockOptions(chestView);
                    break;
                case ChestState.UNLOCKING:
                    ShowSkipTimerOption(chestView);
                    break;
                case ChestState.UNLOCKED:
                    CollectRewards(chestView);
                    break;
            }
        }

        private void ShowUnlockOptions(ChestView chestView)
        {
            Debug.Log("Showing unlock options pop-up");

            var chestModel = chestView.GetChestModel();
            if (!slotController.HasUnlockingChest())
            {
                StartUnlockingChest(chestView);
            }
            else
            {
                var slot = FindSlotWithChestView(chestView);

                if (slot != null && !unlockQueue.Contains(slot))
                {
                    unlockQueue.Enqueue(slot);
                    Debug.Log($"Added chest to unlock queue: {chestModel.ChestScriptableObject.chestType}");
                }
            }
        }

        private void StartUnlockingChest(ChestView chestView)
        {
            var chestModel = chestView.GetChestModel();
            chestModel.CurrentState = ChestState.UNLOCKING;
            chestModel.UnlockStartTime = DateTime.Now;
            chestModel.IsBeingUnlocked = true;

            chestView.UpdateVisuals();

            Debug.Log($"Started unlocking chest: {chestModel.ChestScriptableObject.chestType}");
        }

        private void ShowSkipTimerOption(ChestView chestView)
        {
            var chestModel = chestView.GetChestModel();
            int gemsRequired = chestModel.GetGemsToSkip();

            Debug.Log($"Gems required to skip: {gemsRequired}");

            if (playerController.GemsCount >= gemsRequired)
            {
                lastSkippedChest = chestModel;
                lastSkippedGemCost = gemsRequired;
                canUndo = true;

                playerController.UpdateGemsCount(playerController.GemsCount - gemsRequired);

                chestModel.RemainingTimeInSeconds = 0;
                chestModel.CurrentState = ChestState.UNLOCKED;
                chestView.UpdateVisuals();

                StartNextChestInQueue();

                Debug.Log("Chest unlocked with gems!");
            }
            else
            {
                Debug.Log("Not enough gems to skip timer!");
            }
        }

        private void CollectRewards(ChestView chestView)
        {
            var chestModel = chestView.GetChestModel();

            int goldReward = chestModel.CalculateGoldReward();
            int gemReward = chestModel.CalculateGemReward();

            playerController.UpdateCoinCount(playerController.CoinCount + goldReward);
            playerController.UpdateGemsCount(playerController.GemsCount + gemReward);

            Debug.Log($"Collected rewards - Gold: {goldReward}, Gems: {gemReward}");

            chestModel.CurrentState = ChestState.COLLECTED;
            chestView.UpdateVisuals();

            var slot = FindSlotWithChestView(chestView);
            if (slot != null)
            {
                slotController.RemoveChest(slot);
                chestPool.ReturnItem(chestView);
                chestView.SetActive(false);
            }
        }

        private ChestSlot FindSlotWithChestView(ChestView chestView)
        {
            foreach (var slot in slotController.GetAllSlots())
            {
                if (slot.IsOccupied && slot.currentChestView == chestView)
                    return slot;
            }

            return null;
        }

        private void StartNextChestInQueue()
        {
            if (unlockQueue.Count > 0)
            {
                var nextSlot = unlockQueue.Dequeue();
                if (nextSlot.IsOccupied)
                    StartUnlockingChest(nextSlot.currentChestView);
            }
        }

        public void UndoGemSkip()
        {
            if (!canUndo || lastSkippedChest == null) return;

            playerController.UpdateGemsCount(playerController.GemsCount + lastSkippedGemCost);

            lastSkippedChest.CurrentState = ChestState.UNLOCKING;

            ChestView chestView = null;
            foreach (var slot in slotController.GetAllSlots())
            {
                if (slot.IsOccupied && slot.currentChestModel == lastSkippedChest)
                {
                    chestView = slot.currentChestView;
                    break;
                }
            }

            if (chestView != null)
                chestView.UpdateVisuals();

            canUndo = false;
            lastSkippedChest = null;

            Debug.Log("Undo gem skip successful!");
        }

        public List<ChestSlot> GetAllSlots() => slotController.GetAllSlots();
    }
}