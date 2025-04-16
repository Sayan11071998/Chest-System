using ChestSystem.Chest.Core;
using ChestSystem.Core;
using ChestSystem.UI.Components;
using ChestSystem.UI.Core;
using ChestSystem.Utilities;
using UnityEngine;
using System.Collections;
using ChestSystem.Events;
using ChestSystem.Chest.Data;

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
            chestController.View.UpdateStatusText("UNLOCKING");
            chestController.View.SetGemCostVisible(true);
            StartUnlocking();
        }

        public void OnStateExit()
        {
            chestController.View.SetGemCostVisible(false);
            StopUnlocking();
        }

        public void Update() { }

        public void HandleChestClicked() => ShowInstantUnlockNotification();

        private void ShowInstantUnlockNotification()
        {
            int gemCost = chestController.Model.CurrentGemCost;
            int playerGems = GameService.Instance.playerService.PlayerController.GemsCount;
            string chestType = chestController.View.ChestType.ToString();

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
            int gemCost = chestController.Model.CurrentGemCost;

            if (playerGems >= gemCost)
            {
                GameService.Instance.playerService.PlayerController.UpdateGemsCount(playerGems - gemCost);
                EventService.Instance.OnGemsSpend.InvokeEvent();
                CompleteUnlocking();
            }
        }

        private void CompleteUnlocking()
        {
            chestController.Model.CompleteUnlocking();
            stateMachine.ChangeState(ChestState.UNLOCKED);
        }

        public void OnUnlockTimerComplete() => stateMachine.ChangeState(ChestState.UNLOCKED);

        private void StartUnlocking()
        {
            StopUnlocking();
            unlockCoroutine = chestController.View.StartCoroutine(UnlockTimerCoroutine());
        }

        private void StopUnlocking()
        {
            if (unlockCoroutine != null)
                chestController.View.StopCoroutine(unlockCoroutine);

            unlockCoroutine = null;
        }

        private IEnumerator UnlockTimerCoroutine()
        {
            while (chestController.Model.RemainingUnlockTime > 0)
            {
                yield return new WaitForSeconds(1f);
                chestController.Model.UpdateRemainingTime(1f);
                chestController.View.UpdateTimeAndCost();
            }

            OnUnlockTimerComplete();
        }
    }
}