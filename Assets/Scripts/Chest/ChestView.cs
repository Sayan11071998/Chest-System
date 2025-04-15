using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ChestSystem.Chest.Core;

namespace ChestSystem.Chest.UI
{
    /// <summary>
    /// View component of the Chest MVC pattern.
    /// Handles the visual representation of the chest.
    /// </summary>
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

        /// <summary>
        /// Initialize the chest with data
        /// </summary>
        public void Initialize(ChestScriptableObject chestData)
        {
            this.name = chestData.chestType.ToString();

            // Initialize model
            model.Initialize(chestData);

            // Initialize UI elements
            if (chestImage != null && chestData.chestSprite != null)
                chestImage.sprite = chestData.chestSprite;

            if (gemCostContainer != null)
                gemCostContainer.SetActive(false);

            UpdateStatusText();
            UpdateTimerDisplay();
            UpdateGemCostText();

            // Add listener
            chestButton.onClick.AddListener(OnChestClicked);
        }

        /// <summary>
        /// Handle chest click based on current state
        /// </summary>
        private void OnChestClicked()
        {
            Debug.Log($"Chest clicked: {ChestType}");
            controller.HandleChestClicked();
        }

        /// <summary>
        /// Update the status text based on model state
        /// </summary>
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

        /// <summary>
        /// Update the timer display
        /// </summary>
        public void UpdateTimerDisplay()
        {
            if (timerText != null)
                timerText.text = model.FormatTime();
        }

        /// <summary>
        /// Update the gem cost text
        /// </summary>
        public void UpdateGemCostText()
        {
            if (gemCostText != null)
                gemCostText.text = $"Cost: {model.CurrentGemCost}";
        }

        /// <summary>
        /// Show or hide the gem cost container
        /// </summary>
        public void SetGemCostVisible(bool visible)
        {
            if (gemCostContainer != null)
                gemCostContainer.SetActive(visible);
        }

        /// <summary>
        /// Clean up when returned to pool
        /// </summary>
        public void OnReturnToPool()
        {
            controller.Cleanup();
            chestButton.onClick.RemoveAllListeners();
        }

        private void OnDisable()
        {
            controller.Cleanup();
        }

        /// <summary>
        /// Update the chest state
        /// </summary>
        public void SetState(ChestState newState)
        {
            model.SetState(newState);
            UpdateStatusText();
        }

        /// <summary>
        /// Get the chest data
        /// </summary>
        public ChestScriptableObject GetChestData() => model.GetChestData();
    }
}