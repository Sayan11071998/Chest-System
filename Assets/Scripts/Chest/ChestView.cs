using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ChestSystem.Chest.Core;

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

        private ChestController controller;
        private ChestModel model;

        public ChestType ChestType => model?.ChestType ?? ChestType.COMMON;
        public ChestState CurrentState => model?.CurrentState ?? ChestState.LOCKED;

        private void Awake()
        {
            model = new ChestModel();
            controller = new ChestController(this, model);
        }

        public void Initialize(ChestScriptableObject chestData)
        {
            this.name = chestData.chestType.ToString();

            model.Initialize(chestData);

            if (chestImage != null && chestData.chestSprite != null)
                chestImage.sprite = chestData.chestSprite;

            if (gemCostContainer != null)
                gemCostContainer.SetActive(false);

            UpdateStatusText();
            UpdateTimerDisplay();
            UpdateGemCostText();

            chestButton.onClick.AddListener(OnChestClicked);
        }

        private void OnChestClicked()
        {
            Debug.Log($"Chest clicked: {ChestType}");
            controller.HandleChestClicked();
        }

        public void UpdateStatusText()
        {
            if (statusText != null)
            {
                switch (model.CurrentState)
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

        public void UpdateTimeAndCost()
        {
            UpdateTimerDisplay();
            UpdateGemCostText();
        }

        public void UpdateTimerDisplay()
        {
            if (timerText != null)
                timerText.text = model.FormatTime();
        }

        public void UpdateGemCostText()
        {
            if (gemCostText != null)
                gemCostText.text = $"Cost: {model.CurrentGemCost}";
        }

        public void SetGemCostVisible(bool visible)
        {
            if (gemCostContainer != null)
                gemCostContainer.SetActive(visible);
        }

        public void OnReturnToPool()
        {
            controller.Cleanup();
            chestButton.onClick.RemoveAllListeners();
        }

        private void OnDisable() => controller.Cleanup();

        public void SetState(ChestState newState)
        {
            model.SetState(newState);
            UpdateStatusText();
        }

        public ChestScriptableObject GetChestData() => model.GetChestData();
    }
}