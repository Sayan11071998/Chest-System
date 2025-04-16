using ChestSystem.Chest.Data;
using ChestSystem.Chest.UI;
using ChestSystem.Core;
using ChestSystem.Chest.Utilities;

namespace ChestSystem.Chest.Core
{
    public class ChestController
    {
        private ChestView chestView;
        private ChestModel chestModel;
        private bool isRegisteredAsUnlocking = false;
        private ChestStateMachine chestStateMachine;

        public ChestView ChestView => chestView;
        public ChestModel ChestModel => chestModel;
        public ChestStateMachine ChestStateMachine => chestStateMachine;
        public ChestState CurrentState => chestStateMachine.GetCurrentChestState();

        public ChestController(ChestView view, ChestModel model)
        {
            chestView = view;
            chestModel = model;

            chestStateMachine = new ChestStateMachine(this);
            chestStateMachine.ChangeState(ChestState.LOCKED);
        }

        public void HandleChestClicked() => chestStateMachine.GetCurrentState()?.HandleChestClicked();

        public void SetRegisteredAsUnlocking(bool value) => isRegisteredAsUnlocking = value;

        public void OnUnlockCompleted()
        {
            if (GameService.Instance != null && GameService.Instance.chestService != null)
                GameService.Instance.chestService.OnChestUnlockCompleted(chestView);

            isRegisteredAsUnlocking = false;
        }

        public void Cleanup()
        {
            if (isRegisteredAsUnlocking)
            {
                if (GameService.Instance != null && GameService.Instance.chestService != null)
                    GameService.Instance.chestService.OnChestUnlockCompleted(chestView);

                isRegisteredAsUnlocking = false;
            }
        }

        public void Update() => chestStateMachine.Update();
    }
}