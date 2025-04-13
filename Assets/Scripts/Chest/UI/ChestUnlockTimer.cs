using System.Collections;
using UnityEngine;
using TMPro;
using ChestSystem.Main;

namespace ChestSystem.Chest
{
    public class ChestUnlockTimer
    {
        private float remainingUnlockTime;
        private Coroutine unlockCoroutine;
        private int currentGemCost;
        private TextMeshProUGUI timerText;
        private TextMeshProUGUI gemCostText;
        private GameObject gemCostContainer;
        private ChestView chestView;

        private const float MINUTES_PER_GEM = 10f;

        public ChestUnlockTimer(ChestView chestView)
        {
            this.chestView = chestView;
        }

        public void Initialize(float unlockTimeInSeconds, TextMeshProUGUI timerText, TextMeshProUGUI gemCostText, GameObject gemCostContainer)
        {
            this.remainingUnlockTime = unlockTimeInSeconds;
            this.timerText = timerText;
            this.gemCostText = gemCostText;
            this.gemCostContainer = gemCostContainer;

            UpdateTimerDisplay();
            UpdateGemCost();
        }

        public void AttemptStartUnlocking(ChestView chest)
        {
            if (GameService.Instance.chestService.chestController.CanStartUnlocking())
                StartUnlocking(chest);
            else
                Debug.Log("Another chest is already being unlocked!");
        }

        private void StartUnlocking(ChestView chest)
        {
            StopUnlocking();

            GameService.Instance.chestService.chestController.SetUnlockingChest(chest);
            unlockCoroutine = chestView.StartCoroutine(UnlockTimerCoroutine());
        }

        private IEnumerator UnlockTimerCoroutine()
        {
            chestView.SetState(ChestState.UNLOCKING);

            if (gemCostContainer != null)
                gemCostContainer.SetActive(true);

            while (remainingUnlockTime > 0)
            {
                yield return new WaitForSeconds(1f);
                remainingUnlockTime -= 1f;

                UpdateTimerDisplay();
                UpdateGemCost();
            }

            remainingUnlockTime = 0;
            UpdateTimerDisplay();

            if (gemCostContainer != null)
                gemCostContainer.SetActive(false);

            chestView.SetState(ChestState.UNLOCKED);
            GameService.Instance.chestService.chestController.ChestUnlockCompleted(chestView);
        }

        public void AttemptInstantUnlock()
        {
            int playerGems = GameService.Instance.playerService.PlayerController.GemsCount;

            if (playerGems >= currentGemCost)
            {
                GameService.Instance.playerService.PlayerController.UpdateGemsCount(playerGems - currentGemCost);

                CompleteUnlocking();
            }
            else
            {
                Debug.Log("Not enough gems to instantly unlock chest!");
            }
        }

        private void CompleteUnlocking()
        {
            StopUnlocking();

            remainingUnlockTime = 0;
            UpdateTimerDisplay();

            if (gemCostContainer != null)
                gemCostContainer.SetActive(false);

            chestView.SetState(ChestState.UNLOCKED);
            GameService.Instance.chestService.chestController.ChestUnlockCompleted(chestView);
        }

        public void StopUnlocking()
        {
            if (unlockCoroutine != null)
                chestView.StopCoroutine(unlockCoroutine);

            unlockCoroutine = null;
        }

        private void UpdateGemCost()
        {
            float minutesRemaining = remainingUnlockTime / 60f;
            currentGemCost = Mathf.CeilToInt(minutesRemaining / MINUTES_PER_GEM);

            if (remainingUnlockTime > 0 && currentGemCost == 0)
                currentGemCost = 1;

            if (gemCostText != null)
                gemCostText.text = currentGemCost.ToString();
        }

        private void UpdateTimerDisplay()
        {
            if (timerText != null)
                timerText.text = FormatTime(remainingUnlockTime);
        }

        private string FormatTime(float timeInSeconds)
        {
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
    }
}