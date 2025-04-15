using ChestSystem.Chest.Core;
using ChestSystem.Core;
using ChestSystem.UI.Components;
using ChestSystem.UI.Core;
using ChestSystem.Utilities;
using UnityEngine;

namespace ChestSystem.Chest
{
    public class UnlockingState : IState
    {
        private ChestController chestController;
        private ChestStateMachine stateMachine;

        public UnlockingState(ChestController chestController, ChestStateMachine stateMachine)
        {
            this.chestController = chestController;
            this.stateMachine = stateMachine;
        }

        public void OnStateEnter()
        {
            chestController.View.UpdateStatusText("UNLOCKING");
            chestController.View.SetGemCostVisible(true);
        }

        public void OnStateExit() => chestController.View.SetGemCostVisible(false);

        public void Update()
        {
            // The timer functionality is now handled in the ChestModel
            // but we can use Update() for any continuous state-specific behaviors
        }

        public void HandleChestClicked() => ShowInstantUnlockNotification();

        private void ShowInstantUnlockNotification()
        {
            int gemCost = chestController.Model.CurrentGemCost;
            int playerGems = GameService.Instance.playerService.PlayerController.GemsCount;
            string chestType = chestController.View.ChestType.ToString();

            string title = $"INSTANT UNLOCK - {chestType} CHEST";
            string message;

            if (playerGems >= gemCost)
            {
                message = $"Would you like to instantly unlock this {chestType.ToLower()} chest for {gemCost} gems?\n\nYou have: {playerGems} gems\nCost: {gemCost} gems\n\nTap to confirm!";
                NotificationManager.Instance.ShowNotification(title, message);
                NotificationPanel.OnNotificationClosed += ConfirmInstantUnlock;
            }
            else
            {
                message = $"You don't have enough gems to instantly unlock this chest.\n\nYou have: {playerGems} gems\nRequired: {gemCost} gems\n\nWait for the timer or get more gems!";
                NotificationManager.Instance.ShowNotification(title, message);
            }
        }

        private void ConfirmInstantUnlock()
        {
            NotificationPanel.OnNotificationClosed -= ConfirmInstantUnlock;
            AttemptInstantUnlock();
        }

        private void AttemptInstantUnlock()
        {
            int playerGems = GameService.Instance.playerService.PlayerController.GemsCount;
            int gemCost = chestController.Model.CurrentGemCost;

            if (playerGems >= gemCost)
            {
                GameService.Instance.playerService.PlayerController.UpdateGemsCount(playerGems - gemCost);
                CompleteUnlocking();
            }
            else
            {
                Debug.Log("Not enough gems to instantly unlock chest!");
            }
        }

        private void CompleteUnlocking()
        {
            chestController.Model.CompleteUnlocking();
            stateMachine.ChangeState(ChestState.UNLOCKED);
        }

        public void OnUnlockTimerComplete() => stateMachine.ChangeState(ChestState.UNLOCKED);
    }
}