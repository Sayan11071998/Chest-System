using ChestSystem.Chest.Core;
using ChestSystem.Chest.Utilities;
using ChestSystem.UI.Data;
using ChestSystem.Utilities;

namespace ChestSystem.Chest.States
{
    public class CollectedState : IState
    {
        private ChestController chestController;
        private ChestStateMachine stateMachine;

        public CollectedState(ChestController chestController, ChestStateMachine stateMachine)
        {
            this.chestController = chestController;
            this.stateMachine = stateMachine;
        }

        public void OnStateEnter() => chestController.ChestView.UpdateStatusText(UIStrings.Collected);

        public void OnStateExit() { }

        public void Update() { }

        public void HandleChestClicked() { }
    }
}