using System;
using ChestSystem.Chest.Data;
using UnityEngine;

namespace ChestSystem.Chest.Core
{
    public class ChestModel
    {
        // Core chest properties
        private ChestScriptableObject chestData;
        private ChestState currentState;
        private float remainingUnlockTime;
        private int currentGemCost;
        private Guid id;

        // Events for state changes
        public event Action<ChestState> OnStateChanged;
        public event Action<float> OnUnlockTimeUpdated;
        public event Action<int> OnGemCostUpdated;

        // Properties for read-only access
        public ChestType ChestType => chestData.chestType;
        public Sprite ChestSprite => chestData.chestSprite;
        public float UnlockTimeInSeconds => chestData.unlockTimeInSeconds;
        public int InstantOpenCostInGems => chestData.instantOpenCostInGems;
        public ChestState CurrentState => currentState;
        public float RemainingUnlockTime => remainingUnlockTime;
        public int CurrentGemCost => currentGemCost;
        public Guid Id => id;
        public ChestScriptableObject ChestData => chestData;

        // Constants
        private const float MINUTES_PER_GEM = 10f;

        public ChestModel(ChestScriptableObject chestData)
        {
            this.chestData = chestData;
            this.currentState = ChestState.LOCKED;
            this.remainingUnlockTime = chestData.unlockTimeInSeconds;
            this.id = Guid.NewGuid();
            UpdateGemCost();
        }

        // State management
        public void SetState(ChestState newState)
        {
            if (currentState != newState)
            {
                currentState = newState;
                OnStateChanged?.Invoke(currentState);
            }
        }

        // Unlock timer management
        public void UpdateUnlockTime(float newTime)
        {
            remainingUnlockTime = Mathf.Max(0, newTime);
            OnUnlockTimeUpdated?.Invoke(remainingUnlockTime);
            UpdateGemCost();
        }

        public void DecrementTimer(float deltaTime)
        {
            if (currentState == ChestState.UNLOCKING && remainingUnlockTime > 0)
            {
                UpdateUnlockTime(remainingUnlockTime - deltaTime);

                // Check if timer completed
                if (remainingUnlockTime <= 0)
                {
                    SetState(ChestState.UNLOCKED);
                }
            }
        }

        private void UpdateGemCost()
        {
            float minutesRemaining = remainingUnlockTime / 60f;
            int newGemCost = Mathf.CeilToInt(minutesRemaining / MINUTES_PER_GEM);

            if (remainingUnlockTime > 0 && newGemCost == 0)
                newGemCost = 1;

            if (currentGemCost != newGemCost)
            {
                currentGemCost = newGemCost;
                OnGemCostUpdated?.Invoke(currentGemCost);
            }
        }

        // Reward calculation
        public void CalculateRewards(out int coinsAwarded, out int gemsAwarded)
        {
            coinsAwarded = UnityEngine.Random.Range(chestData.minCoinReward, chestData.maxCoinReward + 1);
            gemsAwarded = UnityEngine.Random.Range(chestData.minGemReward, chestData.maxGemReward + 1);
        }

        // Helper for formatting time display
        public string GetFormattedTimeRemaining()
        {
            if (remainingUnlockTime <= 0)
                return "Ready!";

            if (remainingUnlockTime < 60)
            {
                return string.Format("{0} sec", Mathf.CeilToInt(remainingUnlockTime));
            }
            else if (remainingUnlockTime < 3600)
            {
                int minutes = Mathf.FloorToInt(remainingUnlockTime / 60);
                int seconds = Mathf.FloorToInt(remainingUnlockTime % 60);
                return string.Format("{0}m {1}s", minutes, seconds);
            }
            else
            {
                int hours = Mathf.FloorToInt(remainingUnlockTime / 3600);
                int minutes = Mathf.FloorToInt((remainingUnlockTime % 3600) / 60);
                return string.Format("{0}h {1}m", hours, minutes);
            }
        }
    }
}