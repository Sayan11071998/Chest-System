using ChestSystem.Chest.Core;
using ChestSystem.Chest.Data;
using ChestSystem.Core;
using ChestSystem.Events;
using ChestSystem.UI.Components;
using ChestSystem.UI.Core;
using ChestSystem.Utilities;
using ChestSystem.Chest.Utilities;
using ChestSystem.UI.Data;

namespace ChestSystem.Chest.States
{
    public class UnlockedState : IState
    {
        private ChestStateMachine stateMachine;
        private ChestController chestController;
        private int coinsAwarded;
        private int gemsAwarded;
        private bool rewardsCalculated = false;

        public UnlockedState(ChestController chestController, ChestStateMachine stateMachine)
        {
            this.chestController = chestController;
            this.stateMachine = stateMachine;
        }

        public void OnStateEnter()
        {
            chestController.ChestView.UpdateStatusText(UIStrings.Unlocked);
            chestController.ChestView.UpdateTimerDisplay();
            chestController.ChestView.SetGemCostVisible(false);

            ChestScriptableObject chestData = chestController.ChestModel.GetChestData();
            if (chestData.unlockedChestSprite != null)
                chestController.ChestView.UpdateChestSprite(chestData.unlockedChestSprite);

            chestController.OnUnlockCompleted();

            CalculateRewards();
        }

        public void OnStateExit() { }

        public void Update() { }

        public void HandleChestClicked() => ShowRewardsNotification();

        private void CalculateRewards()
        {
            if (!rewardsCalculated)
            {
                chestController.ChestModel.GetChestData().CalculateRewards(out coinsAwarded, out gemsAwarded);
                rewardsCalculated = true;
            }
        }

        private void ShowRewardsNotification()
        {
            string title = string.Format(UIStrings.ChestRewards, chestController.ChestView.ChestType);
            string message = string.Format(UIStrings.AboutToCollect, coinsAwarded, gemsAwarded);
            string buttonText = UIStrings.Collect;

            NotificationManager.Instance.ShowNotification(title, message, buttonText);
            NotificationPanel.OnNotificationClosed += CollectChestAfterNotification;
        }

        private void CollectChestAfterNotification()
        {
            NotificationPanel.OnNotificationClosed -= CollectChestAfterNotification;

            CollectChest();
        }

        private void CollectChest()
        {
            var playerController = GameService.Instance.playerService.PlayerController;
            playerController.UpdateCoinCount(playerController.CoinCount + coinsAwarded);
            playerController.UpdateGemsCount(playerController.GemsCount + gemsAwarded);

            EventService.Instance.OnChestCollected.InvokeEvent(chestController.ChestView, coinsAwarded, gemsAwarded);
            GameService.Instance.chestService.RemoveChestAndMaintainMinimumSlots(chestController.ChestView, 4);

            stateMachine.ChangeState(ChestState.COLLECTED);
        }
    }
}