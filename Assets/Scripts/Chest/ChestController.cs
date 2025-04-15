using ChestSystem.Chest.UI;
using ChestSystem.Core;

namespace ChestSystem.Chest.Core
{
    public class ChestController
    {
        private ChestView view;
        private ChestModel model;
        private bool isRegisteredAsUnlocking = false;
        private ChestStateMachine stateMachine;

        public ChestView View => view;
        public ChestModel Model => model;
        public ChestStateMachine ChestStateMachine => stateMachine;
        public ChestState CurrentState => stateMachine.GetCurrentChestState();

        public ChestController(ChestView view, ChestModel model)
        {
            this.view = view;
            this.model = model;
            stateMachine = new ChestStateMachine(this);

            stateMachine.ChangeState(ChestState.LOCKED);
        }

        public void HandleChestClicked()
        {
            stateMachine.GetCurrentState()?.HandleChestClicked();
        }

        public void SetRegisteredAsUnlocking(bool value)
        {
            isRegisteredAsUnlocking = value;
        }

        public void OnUnlockCompleted()
        {
            GameService.Instance.chestService.OnChestUnlockCompleted(view);
            isRegisteredAsUnlocking = false;
        }

        public void Cleanup()
        {
            if (isRegisteredAsUnlocking && model.IsUnlocking)
            {
                model.StopUnlocking();
                GameService.Instance.chestService.OnChestUnlockCompleted(view);
                isRegisteredAsUnlocking = false;
            }
        }

        public void Update()
        {
            stateMachine.Update();
        }
    }
}