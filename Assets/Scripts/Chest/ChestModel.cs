using System.Collections;
using ChestSystem.Chest.UI;
using UnityEngine;

namespace ChestSystem.Chest.Core
{
    /// <summary>
    /// Model component of the Chest MVC pattern.
    /// Handles chest data, state, unlock timing, and rewards.
    /// </summary>
    public class ChestModel
    {
        private ChestScriptableObject chestData;
        private ChestState currentState = ChestState.LOCKED;
        private float remainingUnlockTime;
        private int currentGemCost;
        private Coroutine unlockCoroutine;

        // Constants
        private const float MINUTES_PER_GEM = 10f;

        // Properties
        public ChestType ChestType => chestData?.chestType ?? ChestType.COMMON;
        public ChestState CurrentState => currentState;
        public float RemainingUnlockTime => remainingUnlockTime;
        public int CurrentGemCost => currentGemCost;
        public bool IsUnlocking => unlockCoroutine != null;

        /// <summary>
        /// Initialize the chest model with data from a scriptable object
        /// </summary>
        public void Initialize(ChestScriptableObject chestData)
        {
            this.chestData = chestData;
            this.remainingUnlockTime = chestData.unlockTimeInSeconds;
            this.currentState = ChestState.LOCKED;
            UpdateGemCost();
        }

        /// <summary>
        /// Set the chest state and notify any listeners
        /// </summary>
        public void SetState(ChestState newState)
        {
            currentState = newState;
        }

        /// <summary>
        /// Start the unlock process for this chest
        /// </summary>
        public IEnumerator UnlockTimerCoroutine(ChestView view, ChestController controller)
        {
            SetState(ChestState.UNLOCKING);

            while (remainingUnlockTime > 0)
            {
                yield return new WaitForSeconds(1f);
                remainingUnlockTime -= 1f;
                UpdateGemCost();

                // Update the UI
                view.UpdateTimeAndCost();
            }

            remainingUnlockTime = 0;
            SetState(ChestState.UNLOCKED);
            controller.OnUnlockCompleted();
        }

        /// <summary>
        /// Stop the unlock process
        /// </summary>
        public void StopUnlocking(MonoBehaviour coroutineRunner)
        {
            if (unlockCoroutine != null)
                coroutineRunner.StopCoroutine(unlockCoroutine);

            unlockCoroutine = null;
        }

        /// <summary>
        /// Start the unlock process
        /// </summary>
        public void StartUnlocking(MonoBehaviour coroutineRunner, ChestController controller)
        {
            StopUnlocking(coroutineRunner);
            ChestView view = coroutineRunner as ChestView;
            unlockCoroutine = coroutineRunner.StartCoroutine(UnlockTimerCoroutine(view, controller));
        }

        /// <summary>
        /// Complete the unlock immediately
        /// </summary>
        public void CompleteUnlocking(MonoBehaviour coroutineRunner, ChestController controller)
        {
            StopUnlocking(coroutineRunner);
            remainingUnlockTime = 0;
            SetState(ChestState.UNLOCKED);
            UpdateGemCost();
            controller.OnUnlockCompleted();
        }

        /// <summary>
        /// Update the gem cost based on remaining time
        /// </summary>
        private void UpdateGemCost()
        {
            float minutesRemaining = remainingUnlockTime / 60f;
            currentGemCost = Mathf.CeilToInt(minutesRemaining / MINUTES_PER_GEM);

            if (remainingUnlockTime > 0 && currentGemCost == 0)
                currentGemCost = 1;
        }

        /// <summary>
        /// Calculate rewards for this chest
        /// </summary>
        public void CalculateRewards(out int coinsAwarded, out int gemsAwarded)
        {
            coinsAwarded = Random.Range(chestData.minCoinReward, chestData.maxCoinReward + 1);
            gemsAwarded = Random.Range(chestData.minGemReward, chestData.maxGemReward + 1);
        }

        /// <summary>
        /// Format the remaining time as a string
        /// </summary>
        public string FormatTime()
        {
            float timeInSeconds = remainingUnlockTime;

            if (timeInSeconds <= 0)
                return "Ready!";

            if (timeInSeconds < 60)
            {
                return string.Format("{0} sec", Mathf.CeilToInt(timeInSeconds));
            }
            else if (timeInSeconds < 3600)
            {
                int minutes = Mathf.FloorToInt(timeInSeconds / 60);
                int seconds = Mathf.FloorToInt(timeInSeconds % 60);
                return string.Format("{0}m {1}s", minutes, seconds);
            }
            else
            {
                int hours = Mathf.FloorToInt(timeInSeconds / 3600);
                int minutes = Mathf.FloorToInt((timeInSeconds % 3600) / 60);
                return string.Format("{0}h {1}m", hours, minutes);
            }
        }

        /// <summary>
        /// Get the chest's data
        /// </summary>
        public ChestScriptableObject GetChestData() => chestData;
    }
}