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
        [SerializeField] private TextMeshProUGUI gemCostText;
        [SerializeField] private GameObject gemCostContainer;

        private ChestScriptableObject chestData;
        private ChestType chestType;
        private ChestState currentState = ChestState.LOCKED;
        private float remainingUnlockTime;
        private Coroutine unlockCoroutine;
        private int currentGemCost;

        private const float MINUTES_PER_GEM = 10f;

        public void Initialize(ChestScriptableObject chestData)
        {
            this.chestData = chestData;
            this.chestType = chestData.chestType;
            this.remainingUnlockTime = chestData.unlockTimeInSeconds;
            this.name = chestType.ToString();

            if (chestImage != null && chestData.chestSprite != null)
                chestImage.sprite = chestData.chestSprite;

            if (gemCostContainer != null)
                gemCostContainer.SetActive(false);

            SetChestState(ChestState.LOCKED);
            UpdateTimerDisplay();
            UpdateGemCost();
            chestButton.onClick.AddListener(OnChestClicked);
        }

        private void OnChestClicked()
        {
            Debug.Log($"Chest clicked: {chestType}");
            switch (currentState)
            {
                case ChestState.LOCKED:
                    AttemptStartUnlocking();
                    break;
                case ChestState.UNLOCKING:
                    AttemptInstantUnlock();
                    break;
                case ChestState.UNLOCKED:
                    CollectChest();
                    break;
            }
        }

        private void AttemptStartUnlocking()
        {
            // Check if another chest is already being unlocked
            if (GameService.Instance.chestService.chestController.CanStartUnlocking())
            {
                StartUnlocking();
            }
            else
            {
                Debug.Log("Another chest is already being unlocked!");
                // Could show a UI message to the player
            }
        }

        private void StartUnlocking()
        {
            if (unlockCoroutine != null)
                StopCoroutine(unlockCoroutine);

            // Register this chest as the one being unlocked
            GameService.Instance.chestService.chestController.SetUnlockingChest(this);

            unlockCoroutine = StartCoroutine(UnlockTimerCoroutine());
        }

        private IEnumerator UnlockTimerCoroutine()
        {
            SetChestState(ChestState.UNLOCKING);

            // Show gem cost container
            if (gemCostContainer != null)
                gemCostContainer.SetActive(true);

            while (remainingUnlockTime > 0)
            {
                // Wait for one second
                yield return new WaitForSeconds(1f);

                // Decrease the timer
                remainingUnlockTime -= 1f;

                // Update the display
                UpdateTimerDisplay();
                UpdateGemCost();
            }

            // Timer completed
            remainingUnlockTime = 0;
            UpdateTimerDisplay();

            // Hide gem cost container
            if (gemCostContainer != null)
                gemCostContainer.SetActive(false);

            SetChestState(ChestState.UNLOCKED);

            // Notify controller that unlocking is completed
            GameService.Instance.chestService.chestController.ChestUnlockCompleted(this);
        }

        private void UpdateGemCost()
        {
            // Calculate gem cost: 1 gem per 10 minutes (rounded up)
            float minutesRemaining = remainingUnlockTime / 60f;
            currentGemCost = Mathf.CeilToInt(minutesRemaining / MINUTES_PER_GEM);

            // Ensure minimum cost is 1 gem if there's any time left
            if (remainingUnlockTime > 0 && currentGemCost == 0)
                currentGemCost = 1;

            // Update UI
            if (gemCostText != null)
                gemCostText.text = currentGemCost.ToString();
        }

        private void AttemptInstantUnlock()
        {
            // Check if player has enough gems
            int playerGems = GameService.Instance.playerService.PlayerController.GemsCount;

            if (playerGems >= currentGemCost)
            {
                // Deduct gems
                GameService.Instance.playerService.PlayerController.UpdateGemsCount(playerGems - currentGemCost);

                // Complete unlock immediately
                CompleteUnlocking();
            }
            else
            {
                Debug.Log("Not enough gems to instantly unlock chest!");
                // Could display a UI message here
            }
        }

        private void CompleteUnlocking()
        {
            if (unlockCoroutine != null)
                StopCoroutine(unlockCoroutine);

            remainingUnlockTime = 0;
            UpdateTimerDisplay();

            // Hide gem cost container
            if (gemCostContainer != null)
                gemCostContainer.SetActive(false);

            SetChestState(ChestState.UNLOCKED);

            // Notify controller that unlocking is completed
            GameService.Instance.chestService.chestController.ChestUnlockCompleted(this);
        }

        private void CollectChest()
        {
            if (currentState == ChestState.UNLOCKED)
            {
                SetChestState(ChestState.COLLECTED);

                // Get rewards from the chest
                int coinsAwarded, gemsAwarded;
                GameService.Instance.chestService.chestController.CollectChest(this, out coinsAwarded, out gemsAwarded);

                // Update player resources
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
                    statusText.text = "UNLOCKING...";
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