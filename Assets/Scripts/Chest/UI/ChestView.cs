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

        private ChestController controller;
        private ChestModel chestModel;

        public ChestController Controller => controller;
        public ChestType ChestType => chestModel?.ChestType ?? ChestType.COMMON;
        public ChestState CurrentState => controller?.CurrentState ?? ChestState.LOCKED;

        private void Awake()
        {
            chestModel = new ChestModel();
            controller = new ChestController(this, chestModel);
        }

        private void Update() => controller.Update();

        public void Initialize(ChestScriptableObject chestData)
        {
            name = chestData.chestType.ToString();

            chestModel.Initialize(chestData, this);

            if (chestImage != null && chestData.chestSprite != null)
                chestImage.sprite = chestData.chestSprite;

            if (gemCostContainer != null)
                gemCostContainer.SetActive(false);

            UpdateStatusText();
            UpdateTimerDisplay();
            UpdateGemCostText();

            controller.ChestStateMachine.ChangeState(ChestState.LOCKED);
            chestButton.onClick.AddListener(OnChestClicked);
        }

        private void OnChestClicked() => controller.HandleChestClicked();

        public void UpdateStatusText(string text = null)
        {
            if (statusText != null)
            {
                if (text != null)
                {
                    statusText.text = text;
                }
                else
                {
                    switch (controller.CurrentState)
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
            if (timerText != null)
                timerText.text = chestModel.FormatTime();
        }

        public void UpdateGemCostText()
        {
            if (gemCostText != null)
                gemCostText.text = $"Cost: {chestModel.CurrentGemCost}";
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
            controller.Cleanup();
            chestButton.onClick.RemoveAllListeners();
        }

        private void OnDisable() => controller.Cleanup();

        public ChestScriptableObject GetChestData() => chestModel.GetChestData();
    }
}