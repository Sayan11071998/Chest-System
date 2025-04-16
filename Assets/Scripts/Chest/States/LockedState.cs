using ChestSystem.Chest.Core;
using ChestSystem.Core;
using ChestSystem.UI.Core;
using ChestSystem.Utilities;
using UnityEngine;

namespace ChestSystem.Chest
{
    public class LockedState : IState
    {
        private ChestController chestController;
        private ChestStateMachine stateMachine;

        public LockedState(ChestController chestController, ChestStateMachine stateMachine)
        {
            this.chestController = chestController;
            this.stateMachine = stateMachine;
        }

        public void OnStateEnter()
        {
            chestController.View.SetGemCostVisible(false);
            chestController.View.UpdateStatusText("LOCKED");
        }

        public void OnStateExit() { }

        public void Update() { }

        public void HandleChestClicked()
        {
            var chestService = GameService.Instance.chestService;

            if (chestService.CanStartUnlocking())
            {
                chestService.SetUnlockingChest(chestController.View);
                chestController.View.SetGemCostVisible(true);
                chestController.View.UpdateStatusText("UNLOCKING");
                chestController.SetRegisteredAsUnlocking(true);

                stateMachine.ChangeState(ChestState.UNLOCKING);
            }
            else
            {
                string title = "CHEST LOCKED";
                string message = "Another chest is already being unlocked!";
                NotificationManager.Instance.ShowNotification(title, message);
                Debug.Log("Another chest is already being unlocked!");
            }
        }
    }
}