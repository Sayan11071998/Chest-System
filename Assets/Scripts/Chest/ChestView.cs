using System.Collections;
using ChestSystem.Main;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChestSystem.Chest
{
    public class ChestView : MonoBehaviour
    {
        [SerializeField] private Image chestImage;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private Button chestButton;

        private ChestScriptableObject chestData;
        private ChestType chestType;
        private ChestState currentState = ChestState.LOCKED;
        private float remainingUnlockTime;
        private Coroutine unlockCoroutine;

        public void Initialize(ChestScriptableObject chestData)
        {
            this.chestData = chestData;
            this.chestType = chestData.chestType;
            this.remainingUnlockTime = chestData.unlockTimeInSeconds;
            this.name = chestType.ToString();

            InitializeChest(chestData);
        }

        private void InitializeChest(ChestScriptableObject chestData)
        {
            if (chestImage != null && chestData.chestSprite != null)
                chestImage.sprite = chestData.chestSprite;

            SetChestState(ChestState.LOCKED);
            UpdateTimerDisplay();
            chestButton.onClick.AddListener(OnChestClicked);
        }

        private void OnChestClicked()
        {
            Debug.Log($"Chest clicked: {chestType}");

            switch (currentState)
            {
                case ChestState.LOCKED:
                    StartUnlocking();
                    break;
                case ChestState.UNLOCKING:
                    AttemptInstantUnlock();
                    break;
                case ChestState.UNLOCKED:
                    CollectChest();
                    break;
            }
        }

        private void StartUnlocking()
        {
            if (unlockCoroutine != null)
                StopCoroutine(unlockCoroutine);

            unlockCoroutine = StartCoroutine(UnlockTimerCoroutine());
        }

        private IEnumerator UnlockTimerCoroutine()
        {
            SetChestState(ChestState.UNLOCKING);

            while (remainingUnlockTime > 0)
            {
                yield return new WaitForSeconds(1f);
                remainingUnlockTime -= 1f;
                UpdateTimerDisplay();
            }

            remainingUnlockTime = 0;
            UpdateTimerDisplay();
            SetChestState(ChestState.UNLOCKED);
        }

        private void AttemptInstantUnlock()
        {
            int gemsRequired = chestData.instantOpenCostInGems;
            int playerGems = GameService.Instance.playerService.PlayerController.GemsCount;

            if (playerGems >= gemsRequired)
            {
                GameService.Instance.playerService.PlayerController.UpdateGemsCount(playerGems - gemsRequired);

                if (unlockCoroutine != null)
                    StopCoroutine(unlockCoroutine);

                remainingUnlockTime = 0;
                UpdateTimerDisplay();
                SetChestState(ChestState.UNLOCKED);
            }
            else
            {
                Debug.Log("Not enough gems to instantly unlock chest!");
            }
        }

        private void CollectChest()
        {
            if (currentState == ChestState.UNLOCKED)
            {
                SetChestState(ChestState.COLLECTED);

                int coinsAwarded;
                int gemsAwarded;
                GameService.Instance.chestService.chestController.CollectChest(this, out coinsAwarded, out gemsAwarded);

                var playerController = GameService.Instance.playerService.PlayerController;
                playerController.UpdateCoinCount(playerController.CoinCount + coinsAwarded);
                playerController.UpdateGemsCount(playerController.GemsCount + gemsAwarded);

                Debug.Log($"Collected chest: {chestType}. Rewards: {coinsAwarded} coins, {gemsAwarded} gems");
            }
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

        private void SetChestState(ChestState newState)
        {
            currentState = newState;

            switch (currentState)
            {
                case ChestState.LOCKED:
                    statusText.text = "LOCKED";
                    break;
                case ChestState.UNLOCKING:
                    statusText.text = "UNLOCKING";
                    break;
                case ChestState.UNLOCKED:
                    statusText.text = "UNLOCKED";
                    break;
                case ChestState.COLLECTED:
                    statusText.text = "COLLECTED";
                    break;
            }
        }

        public void OnReturnToPool()
        {
            if (unlockCoroutine != null)
                StopCoroutine(unlockCoroutine);

            chestButton.onClick.RemoveAllListeners();
        }

        private void OnDisable()
        {
            if (unlockCoroutine != null)
                StopCoroutine(unlockCoroutine);
        }
    }
}