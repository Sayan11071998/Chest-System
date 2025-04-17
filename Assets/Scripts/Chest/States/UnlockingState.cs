using ChestSystem.Chest.Core;
using ChestSystem.Core;
using ChestSystem.UI.Components;
using ChestSystem.UI.Core;
using ChestSystem.Utilities;
using UnityEngine;
using System.Collections;
using ChestSystem.Events;
using ChestSystem.Chest.Data;
using ChestSystem.Chest.Utilities;
using ChestSystem.Command.ConcreteCommand;

namespace ChestSystem.Chest.States
{
    public class UnlockingState : IState
    {
        private ChestController chestController;
        private ChestStateMachine stateMachine;
        private Coroutine unlockCoroutine;

        public UnlockingState(ChestController chestController, ChestStateMachine stateMachine)
        {
            this.chestController = chestController;
            this.stateMachine = stateMachine;
        }

        public void OnStateEnter()
        {
            chestController.ChestView.UpdateStatusText("UNLOCKING");
            chestController.ChestView.SetGemCostVisible(true);
            StartUnlocking();
        }

        public void OnStateExit()
        {
            chestController.ChestView.SetGemCostVisible(false);
            StopUnlocking();
        }

        public void Update() { }

        public void HandleChestClicked() => ShowInstantUnlockNotification();

        private void ShowInstantUnlockNotification()
        {
            int gemCost = chestController.ChestModel.CurrentGemCost;
            int playerGems = GameService.Instance.playerService.PlayerController.GemsCount;
            string chestType = chestController.ChestView.ChestType.ToString();

            string title = $"INSTANT UNLOCK - {chestType} CHEST";
            string message;
            string buttonText;

            if (playerGems >= gemCost)
            {
                message = $"Would you like to instantly unlock this {chestType.ToLower()} chest for {gemCost} gems?\n\nYou have: {playerGems} gems\nCost: {gemCost} gems\n\nTap to confirm!";
                buttonText = "CONFIRM";
                NotificationManager.Instance.ShowNotification(title, message, buttonText);
                NotificationPanel.OnNotificationClosed += ConfirmInstantUnlock;
            }
            else
            {
                message = $"You don't have enough gems to instantly unlock this chest.\n\nYou have: {playerGems} gems\nRequired: {gemCost} gems\n\nWait for the timer or get more gems!";
                buttonText = "OKAY";
                NotificationManager.Instance.ShowNotification(title, message, buttonText);
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
            int gemCost = chestController.ChestModel.CurrentGemCost;

            if (playerGems >= gemCost)
            {
                InstantChestUnlockCommand unlockCommand = new InstantChestUnlockCommand();
                unlockCommand.Execute(GameService.Instance.playerService, chestController);
                GameService.Instance.commandInvoker.ExecuteCommand(unlockCommand);
                EventService.Instance.OnGemsSpend.InvokeEvent();
            }
        }

        private void CompleteUnlocking()
        {
            chestController.ChestModel.CompleteUnlocking();
            stateMachine.ChangeState(ChestState.UNLOCKED);
        }

        public void OnUnlockTimerComplete() => stateMachine.ChangeState(ChestState.UNLOCKED);

        private void StartUnlocking()
        {
            StopUnlocking();
            unlockCoroutine = chestController.ChestView.StartCoroutine(UnlockTimerCoroutine());
        }

        private void StopUnlocking()
        {
            if (unlockCoroutine != null)
                chestController.ChestView.StopCoroutine(unlockCoroutine);

            unlockCoroutine = null;
        }

        private IEnumerator UnlockTimerCoroutine()
        {
            while (chestController.ChestModel.RemainingUnlockTime > 0)
            {
                yield return new WaitForSeconds(1f);
                chestController.ChestModel.UpdateRemainingTime(1f);
                chestController.ChestView.UpdateTimeAndCost();
            }

            OnUnlockTimerComplete();
        }
    }
}