using ChestSystem.Chest.Core;
using ChestSystem.Utilities;

namespace ChestSystem.Chest.States
{
    public class CollectedState : IState
    {
        private ChestStateMachine stateMachine;
        private ChestController chestController;

        public CollectedState(ChestController chestController, ChestStateMachine stateMachine)
        {
            this.chestController = chestController;
            this.stateMachine = stateMachine;
        }

        public void OnStateEnter() => chestController.View.UpdateStatusText("COLLECTED");

        public void OnStateExit() { }

        public void Update() { }

        public void HandleChestClicked() { }
    }
}