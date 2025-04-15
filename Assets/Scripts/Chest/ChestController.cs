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
            switch (stateMachine.GetCurrentChestState())
            {
                case ChestState.LOCKED:
                    (stateMachine.GetStates()[ChestState.LOCKED] as LockedState)?.HandleChestClicked();
                    break;

                case ChestState.UNLOCKING:
                    (stateMachine.GetStates()[ChestState.UNLOCKING] as UnlockingState)?.HandleChestClicked();
                    break;

                case ChestState.UNLOCKED:
                    (stateMachine.GetStates()[ChestState.UNLOCKED] as UnlockedState)?.HandleChestClicked();
                    break;
            }
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