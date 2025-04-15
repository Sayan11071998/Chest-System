using System.Collections;
using ChestSystem.Chest.UI;
using UnityEngine;

namespace ChestSystem.Chest.Core
{
    public class ChestModel
    {
        private ChestScriptableObject chestData;
        private float remainingUnlockTime;
        private int currentGemCost;
        private Coroutine unlockCoroutine;
        private ChestView view;

        private const float MINUTES_PER_GEM = 10f;

        public ChestType ChestType => chestData?.chestType ?? ChestType.COMMON;
        public float RemainingUnlockTime => remainingUnlockTime;
        public int CurrentGemCost => currentGemCost;
        public bool IsUnlocking => unlockCoroutine != null;

        public void Initialize(ChestScriptableObject chestData, ChestView view)
        {
            this.chestData = chestData;
            this.remainingUnlockTime = chestData.unlockTimeInSeconds;
            this.view = view;

            UpdateGemCost();
        }

        private void UpdateGemCost()
        {
            float minutesRemaining = remainingUnlockTime / 60f;
            currentGemCost = Mathf.CeilToInt(minutesRemaining / MINUTES_PER_GEM);

            if (remainingUnlockTime > 0 && currentGemCost == 0)
                currentGemCost = 1;
        }

        public IEnumerator UnlockTimerCoroutine()
        {
            while (remainingUnlockTime > 0)
            {
                yield return new WaitForSeconds(1f);
                remainingUnlockTime -= 1f;
                UpdateGemCost();

                view.UpdateTimeAndCost();
            }

            remainingUnlockTime = 0;

            UnlockingState unlockingState = view.Controller.ChestStateMachine.GetStates()[ChestState.UNLOCKING] as UnlockingState;
            unlockingState?.OnUnlockTimerComplete();
        }

        public void StopUnlocking()
        {
            if (unlockCoroutine != null)
                view.StopCoroutine(unlockCoroutine);

            unlockCoroutine = null;
        }

        public void StartUnlocking()
        {
            StopUnlocking();
            unlockCoroutine = view.StartCoroutine(UnlockTimerCoroutine());
        }

        public void CompleteUnlocking()
        {
            StopUnlocking();
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
                return string.Format("{0}h {1}m", hours, minutes);
            }
        }

        public ChestScriptableObject GetChestData() => chestData;
    }
}