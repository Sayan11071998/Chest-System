using ChestSystem.Chest.Core;
using ChestSystem.Chest.Data;
using ChestSystem.Core;
using ChestSystem.UI.Core;
using ChestSystem.Utilities;
using ChestSystem.Chest.Utilities;

namespace ChestSystem.Chest.States
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
            chestController.ChestView.SetGemCostVisible(false);
            chestController.ChestView.UpdateStatusText("LOCKED");
        }

        public void OnStateExit() { }

        public void Update() { }

        public void HandleChestClicked()
        {
            var chestService = GameService.Instance.chestService;

            if (chestService.CanStartUnlocking())
            {
                chestService.SetUnlockingChest(chestController.ChestView);
                chestController.ChestView.SetGemCostVisible(true);
                chestController.ChestView.UpdateStatusText("UNLOCKING");
                chestController.SetRegisteredAsUnlocking(true);

                stateMachine.ChangeState(ChestState.UNLOCKING);
            }
            else
            {
                string title = "CHEST LOCKED";
                string message = "Another chest is already being unlocked!";
                string buttonText = "CLOSE";
                NotificationManager.Instance.ShowNotification(title, message, buttonText);
            }
        }
    }
}