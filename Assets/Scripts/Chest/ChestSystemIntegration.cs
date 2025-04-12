using UnityEngine;
using ChestSystem.Chest;
using ChestSystem.Player;
using ChestSystem.UI;
using ChestSystem.Main;
using UnityEngine.UI;

namespace ChestSystem
{
    public class ChestSystemIntegration : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private UISlotManager slotManager;
        [SerializeField] private ChestUIManager uiManager;
        [SerializeField] private Button generateChestButton;
        [SerializeField] private Button undoButton;

        private ChestController chestController;
        private PlayerController playerController;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            chestController = GameService.Instance.chestService.chestController;
            playerController = GameService.Instance.playerService.PlayerController;
            slotManager.Initialize(chestController);

            if (generateChestButton != null)
                generateChestButton.onClick.AddListener(OnGenerateChestClicked);

            if (undoButton != null)
                undoButton.onClick.AddListener(OnUndoClicked);
        }

        private void Update() => slotManager.UpdateSlotsFromController();

        private void OnGenerateChestClicked()
        {
            if (!chestController.HasEmptySlot())
            {
                uiManager.ShowNoSlotsPopup();
                return;
            }

            chestController.GenerateRandomChest();
        }

        private void OnUndoClicked()
        {
            chestController.UndoGemSkip();
        }

        public void OnChestClicked(ChestView chestView)
        {
            ChestModel model = chestView.GetChestModel();

            switch (model.CurrentState)
            {
                case ChestState.LOCKED:
                    uiManager.ShowChestOptionsPopup(
                        chestView,
                        () => StartUnlockingChest(chestView),
                        () => SkipTimerWithGems(chestView)
                    );
                    break;

                case ChestState.UNLOCKING:
                    uiManager.ShowSkipTimerPopup(
                        chestView,
                        () => SkipTimerWithGems(chestView),
                        () => chestController.UndoGemSkip()
                    );
                    break;

                case ChestState.UNLOCKED:
                    int goldReward = model.CalculateGoldReward();
                    int gemReward = model.CalculateGemReward();

                    uiManager.ShowRewardPanel(
                        goldReward,
                        gemReward,
                        () => CollectChestReward(chestView)
                    );
                    break;
            }
        }

        private void StartUnlockingChest(ChestView chestView)
        {
            uiManager.HideAllPanels();
            chestController.OnChestClicked(chestView);
        }

        private void SkipTimerWithGems(ChestView chestView)
        {
            ChestModel model = chestView.GetChestModel();
            int gemsRequired = model.GetGemsToSkip();

            if (playerController.GemsCount < gemsRequired)
            {
                uiManager.ShowNotEnoughGemsPopup();
                return;
            }

            uiManager.HideAllPanels();
            chestController.OnChestClicked(chestView);
        }

        private void CollectChestReward(ChestView chestView)
        {
            uiManager.HideAllPanels();
            chestController.OnChestClicked(chestView);
        }
    }
}