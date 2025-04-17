using ChestSystem.Chest.Core;
using ChestSystem.Chest.Data;
using ChestSystem.Core;
using ChestSystem.UI.Core;
using ChestSystem.Utilities;
using ChestSystem.Chest.Utilities;
using ChestSystem.UI.Data;

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
            chestController.ChestView.UpdateStatusText(UIStrings.Locked);
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
                chestController.ChestView.UpdateStatusText(UIStrings.Unlocking);
                chestController.SetRegisteredAsUnlocking(true);

                stateMachine.ChangeState(ChestState.UNLOCKING);
            }
            else
            {
                string title = UIStrings.ChestLocked;
                string message = UIStrings.AnotherChestIsAlreadyBeingUnlocked;
                string buttonText = UIStrings.Close;
                NotificationManager.Instance.ShowNotification(title, message, buttonText);
            }
        }
    }
}