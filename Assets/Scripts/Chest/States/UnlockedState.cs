using ChestSystem.Chest.Core;
using ChestSystem.Core;
using ChestSystem.Events;
using ChestSystem.UI.Components;
using ChestSystem.UI.Core;
using ChestSystem.Utilities;
using UnityEngine;

namespace ChestSystem.Chest
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
            chestController.View.UpdateStatusText("UNLOCKED");
            chestController.View.UpdateTimerDisplay();
            chestController.View.SetGemCostVisible(false);

            ChestScriptableObject chestData = chestController.Model.GetChestData();
            if (chestData.unlockedChestSprite != null)
                chestController.View.UpdateChestSprite(chestData.unlockedChestSprite);

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
                chestController.Model.GetChestData().CalculateRewards(out coinsAwarded, out gemsAwarded);
                rewardsCalculated = true;
            }
        }

        private void ShowRewardsNotification()
        {
            string title = $"{chestController.View.ChestType} CHEST REWARDS";
            string message = $"You are about to collect:\n\n{coinsAwarded} coins\n{gemsAwarded} gems\n\nTap to collect!";

            NotificationManager.Instance.ShowNotification(title, message);
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

            EventService.Instance.OnChestCollected.InvokeEvent(chestController.View, coinsAwarded, gemsAwarded);
            GameService.Instance.chestService.ReplaceChestWithEmptySlot(chestController.View);
            Debug.Log($"Collected chest: {chestController.View.ChestType}. Rewards: {coinsAwarded} coins, {gemsAwarded} gems");

            stateMachine.ChangeState(ChestState.COLLECTED);
        }
    }
}