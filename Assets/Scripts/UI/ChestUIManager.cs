using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ChestSystem.UI
{
    public class ChestUIManager : MonoBehaviour
    {
        [Header("Popups")]
        [SerializeField] private GameObject chestOptionsPopup;
        [SerializeField] private GameObject noSlotsPopup;
        [SerializeField] private GameObject notEnoughGemsPopup;

        [Header("Chest Options Panel")]
        [SerializeField] private Button startTimerButton;
        [SerializeField] private Button unlockWithGemsButton;
        [SerializeField] private TextMeshProUGUI chestTypeText;
        [SerializeField] private TextMeshProUGUI unlockTimeText;
        [SerializeField] private TextMeshProUGUI gemCostText;

        [Header("Skip Timer Panel")]
        [SerializeField] private GameObject skipTimerPanel;
        [SerializeField] private Button skipTimerButton;
        [SerializeField] private Button cancelSkipButton;
        [SerializeField] private TextMeshProUGUI skipGemCostText;
        [SerializeField] private Button undoButton;

        [Header("Reward Panel")]
        [SerializeField] private GameObject rewardPanel;
        [SerializeField] private TextMeshProUGUI goldRewardText;
        [SerializeField] private TextMeshProUGUI gemRewardText;
        [SerializeField] private Button collectRewardButton;

        private Action onStartTimer;
        private Action onSkipTimer;
        private Action onCollectReward;
        private Action onUndoSkip;
        private Chest.ChestView currentSelectedChest;

        private void Start()
        {
            SetupButtonListeners();
            HideAllPanels();
        }

        private void SetupButtonListeners()
        {
            if (startTimerButton != null)
                startTimerButton.onClick.AddListener(() => onStartTimer?.Invoke());

            if (unlockWithGemsButton != null)
                unlockWithGemsButton.onClick.AddListener(() => onSkipTimer?.Invoke());

            if (skipTimerButton != null)
                skipTimerButton.onClick.AddListener(() => onSkipTimer?.Invoke());

            if (collectRewardButton != null)
                collectRewardButton.onClick.AddListener(() => onCollectReward?.Invoke());

            if (undoButton != null)
                undoButton.onClick.AddListener(() => onUndoSkip?.Invoke());

            if (cancelSkipButton != null)
                cancelSkipButton.onClick.AddListener(HideAllPanels);
        }

        public void ShowChestOptionsPopup(Chest.ChestView chestView, Action startTimer, Action skipTimer)
        {
            currentSelectedChest = chestView;
            onStartTimer = startTimer;
            onSkipTimer = skipTimer;

            var chestModel = chestView.GetChestModel();

            if (chestTypeText != null)
                chestTypeText.text = chestModel.ChestScriptableObject.chestType.ToString();

            if (unlockTimeText != null)
            {
                float hours = chestModel.ChestScriptableObject.unlockTimeInSeconds / 3600f;
                unlockTimeText.text = $"{hours:F1} Hours";
            }

            if (gemCostText != null)
                gemCostText.text = chestModel.ChestScriptableObject.instantOpenCostInGems.ToString();

            HideAllPanels();

            if (chestOptionsPopup != null)
                chestOptionsPopup.SetActive(true);
        }

        public void ShowSkipTimerPopup(Chest.ChestView chestView, Action skipTimer, Action undoSkip)
        {
            currentSelectedChest = chestView;
            onSkipTimer = skipTimer;
            onUndoSkip = undoSkip;

            var chestModel = chestView.GetChestModel();

            if (skipGemCostText != null)
                skipGemCostText.text = chestModel.GetGemsToSkip().ToString();

            HideAllPanels();

            if (skipTimerPanel != null)
                skipTimerPanel.SetActive(true);
        }

        public void ShowRewardPanel(int goldReward, int gemReward, Action collectReward)
        {
            onCollectReward = collectReward;

            if (goldRewardText != null)
                goldRewardText.text = goldReward.ToString();

            if (gemRewardText != null)
                gemRewardText.text = gemReward.ToString();

            HideAllPanels();

            if (rewardPanel != null)
                rewardPanel.SetActive(true);
        }

        public void ShowNotEnoughGemsPopup()
        {
            HideAllPanels();

            if (notEnoughGemsPopup != null)
                notEnoughGemsPopup.SetActive(true);
        }

        public void ShowNoSlotsPopup()
        {
            HideAllPanels();

            if (noSlotsPopup != null)
                noSlotsPopup.SetActive(true);
        }

        public void HideAllPanels()
        {
            if (chestOptionsPopup != null)
                chestOptionsPopup.SetActive(false);

            if (skipTimerPanel != null)
                skipTimerPanel.SetActive(false);

            if (rewardPanel != null)
                rewardPanel.SetActive(false);

            if (noSlotsPopup != null)
                noSlotsPopup.SetActive(false);

            if (notEnoughGemsPopup != null)
                notEnoughGemsPopup.SetActive(false);
        }
    }
}