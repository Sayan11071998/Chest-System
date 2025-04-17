using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ChestSystem.Chest.Core;
using ChestSystem.Chest.Data;

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

        private ChestController chestController;

        public ChestController ChestController => chestController;
        public ChestType ChestType => chestController?.ChestModel.ChestType ?? ChestType.COMMON;
        public ChestState CurrentState => chestController?.CurrentState ?? ChestState.LOCKED;

        private void Update()
        {
            if (chestController != null)
                chestController.Update();
        }

        public void SetController(ChestController controller)
        {
            chestController = controller;
            chestButton.onClick.AddListener(OnChestClicked);
        }

        public void Initialize(ChestType chestType)
        {
            name = chestType.ToString();

            if (gemCostContainer != null)
                gemCostContainer.SetActive(false);

            UpdateStatusText("LOCKED");
        }

        private void OnChestClicked()
        {
            if (chestController != null)
                chestController.HandleChestClicked();
        }

        public void UpdateStatusText(string text = null)
        {
            if (statusText != null)
            {
                if (text != null)
                {
                    statusText.text = text;
                }
                else if (chestController != null)
                {
                    switch (chestController.CurrentState)
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
            }
        }

        public void UpdateTimeAndCost()
        {
            UpdateTimerDisplay();
            UpdateGemCostText();
        }

        public void UpdateTimerDisplay()
        {
            if (timerText != null && chestController != null)
                timerText.text = chestController.ChestModel.FormatTime();
        }

        public void UpdateGemCostText()
        {
            if (gemCostText != null && chestController != null)
                gemCostText.text = $"Cost: {chestController.ChestModel.CurrentGemCost}";
        }

        public void SetGemCostVisible(bool visible)
        {
            if (gemCostContainer != null)
                gemCostContainer.SetActive(visible);
        }

        public void UpdateChestSprite(Sprite newSprite)
        {
            if (chestImage != null && newSprite != null)
                chestImage.sprite = newSprite;
        }

        public void OnReturnToPool()
        {
            if (chestController != null)
                chestController.Cleanup();

            chestButton.onClick.RemoveAllListeners();
            chestController = null;
        }

        private void OnDisable()
        {
            if (chestController != null)
                chestController.Cleanup();
        }

        public ChestScriptableObject GetChestData() => chestController?.ChestModel.GetChestData();
    }
}