using ChestSystem.Chest.UI;
using UnityEngine;

namespace ChestSystem.Chest.Core
{
    public class ChestModel
    {
        private ChestScriptableObject chestData;
        private float remainingUnlockTime;
        private int currentGemCost;
        private ChestView view;

        private const float MINUTES_PER_GEM = 10f;

        public ChestType ChestType => chestData?.chestType ?? ChestType.COMMON;
        public float RemainingUnlockTime => remainingUnlockTime;
        public int CurrentGemCost => currentGemCost;

        public void Initialize(ChestScriptableObject chestData, ChestView view)
        {
            this.chestData = chestData;
            this.remainingUnlockTime = chestData.unlockTimeInSeconds;
            this.view = view;

            UpdateGemCost();
        }

        public void UpdateGemCost()
        {
            float minutesRemaining = remainingUnlockTime / 60f;
            currentGemCost = Mathf.CeilToInt(minutesRemaining / MINUTES_PER_GEM);

            if (remainingUnlockTime > 0 && currentGemCost == 0)
                currentGemCost = 1;
        }

        public void UpdateRemainingTime(float timeDecrement)
        {
            remainingUnlockTime -= timeDecrement;
            if (remainingUnlockTime < 0)
                remainingUnlockTime = 0;

            UpdateGemCost();
        }

        public void CompleteUnlocking()
        {
            remainingUnlockTime = 0;
            UpdateGemCost();
        }

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
                int seconds = Mathf.FloorToInt(timeInSeconds % 60);
                return string.Format("{0}h {1}m {2}s", hours, minutes, seconds);
            }
        }


        public ChestScriptableObject GetChestData() => chestData;
    }
}