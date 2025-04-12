using System;
using UnityEngine;

namespace ChestSystem.Chest
{
    public class ChestModel
    {
        private ChestScriptableObject chestScriptableObject;
        private ChestState currentState;
        private float remainingTimeInSeconds;
        private DateTime unlockStartTime;
        private bool isBeingUnlocked;

        public ChestScriptableObject ChestScriptableObject => chestScriptableObject;

        public ChestState CurrentState
        {
            get => currentState;
            set => currentState = value;
        }

        public float RemainingTimeInSeconds
        {
            get => remainingTimeInSeconds;
            set => remainingTimeInSeconds = value;
        }

        public DateTime UnlockStartTime
        {
            get => unlockStartTime;
            set => unlockStartTime = value;
        }

        public bool IsBeingUnlocked
        {
            get => isBeingUnlocked;
            set => isBeingUnlocked = value;
        }

        public ChestModel(ChestScriptableObject _chestScriptableObject)
        {
            chestScriptableObject = _chestScriptableObject;
            currentState = ChestState.LOCKED;
            remainingTimeInSeconds = chestScriptableObject.unlockTimeInSeconds;
        }

        public int GetGemsToSkip()
        {
            float minutes = remainingTimeInSeconds / 60f;
            float gems = minutes / 10f;

            return Mathf.CeilToInt(gems);
        }

        public int CalculateGoldReward() => CalculateRandomNumber(chestScriptableObject.minCoinReward, chestScriptableObject.maxCoinReward);
        public int CalculateGemReward() => CalculateRandomNumber(chestScriptableObject.minGemReward, chestScriptableObject.maxGemReward);

        public int CalculateRandomNumber(int num1, int num2) => UnityEngine.Random.Range(num1, num2 + 1);
    }
}