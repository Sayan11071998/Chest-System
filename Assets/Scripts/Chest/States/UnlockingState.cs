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
using ChestSystem.UI.Data;
using System;

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
            chestController.ChestView.UpdateStatusText(UIStrings.Unlocking);
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

            string title = string.Format(UIStrings.InstantUnlockChest, chestType);
            string message;
            string buttonText;

            if (playerGems >= gemCost)
            {
                message = string.Format(UIStrings.WouldLikeInstantUnlock, chestType.ToLower(), gemCost, playerGems);
                buttonText = UIStrings.Confirm;
                NotificationManager.Instance.ShowNotification(title, message, buttonText);
                NotificationPanel.OnNotificationClosed += ConfirmInstantUnlock;
            }
            else
            {
                message = String.Format(UIStrings.DoNotHaveEnoughGems, playerGems, gemCost);
                buttonText = UIStrings.Okay;
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