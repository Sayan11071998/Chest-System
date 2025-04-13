using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ChestSystem.Chest.Managers;

namespace ChestSystem.Chest.UI
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
        private ChestStateManager stateManager;
        private ChestUnlockTimer unlockTimer;
        private ChestRewardManager rewardManager;

        public ChestType ChestType => chestData?.chestType ?? ChestType.COMMON;
        public ChestState CurrentState => stateManager?.CurrentState ?? ChestState.LOCKED;

        private void Awake()
        {
            stateManager = new ChestStateManager(this);
            unlockTimer = new ChestUnlockTimer(this);
            rewardManager = new ChestRewardManager(this);
        }

        public void Initialize(ChestScriptableObject chestData)
        {
            this.chestData = chestData;
            this.name = chestData.chestType.ToString();

            if (chestImage != null && chestData.chestSprite != null)
                chestImage.sprite = chestData.chestSprite;

            if (gemCostContainer != null)
                gemCostContainer.SetActive(false);

            stateManager.Initialize(statusText);
            unlockTimer.Initialize(chestData.unlockTimeInSeconds, timerText, gemCostText, gemCostContainer);
            rewardManager.Initialize(chestData);

            chestButton.onClick.AddListener(OnChestClicked);
        }

        private void OnChestClicked()
        {
            Debug.Log($"Chest clicked: {ChestType}");

            switch (stateManager.CurrentState)
            {
                case ChestState.LOCKED:
                    unlockTimer.AttemptStartUnlocking(this);
                    break;

                case ChestState.UNLOCKING:
                    unlockTimer.AttemptInstantUnlock();
                    break;

                case ChestState.UNLOCKED:
                    rewardManager.CollectChest(this);
                    stateManager.SetChestState(ChestState.COLLECTED);
                    break;
            }
        }

        public void OnReturnToPool()
        {
            unlockTimer.StopUnlocking();
            chestButton.onClick.RemoveAllListeners();
        }

        private void OnDisable() => unlockTimer.StopUnlocking();

        public void SetState(ChestState newState) => stateManager.SetChestState(newState);
        public ChestScriptableObject GetChestData() => chestData;
    }
}