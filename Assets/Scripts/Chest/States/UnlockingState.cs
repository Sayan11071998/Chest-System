using ChestSystem.Chest.Core;
using ChestSystem.Core;
using ChestSystem.Utilities;
using UnityEngine;

namespace ChestSystem.Chest
{
    public class UnlockingState : IState
    {
        private ChestController chestController;
        private ChestStateMachine stateMachine;

        public UnlockingState(ChestController chestController, ChestStateMachine stateMachine)
        {
            this.chestController = chestController;
            this.stateMachine = stateMachine;
        }

        public void OnStateEnter()
        {
            chestController.View.UpdateStatusText("UNLOCKING");
            chestController.View.SetGemCostVisible(true);
        }

        public void OnStateExit() => chestController.View.SetGemCostVisible(false);

        public void Update()
        {
            // The timer functionality is now handled in the ChestModel
            // but we can use Update() for any continuous state-specific behaviors
        }

        public void HandleChestClicked() => AttemptInstantUnlock();

        private void AttemptInstantUnlock()
        {
            int playerGems = GameService.Instance.playerService.PlayerController.GemsCount;

            if (playerGems >= chestController.Model.CurrentGemCost)
            {
                GameService.Instance.playerService.PlayerController.UpdateGemsCount(playerGems - chestController.Model.CurrentGemCost);
                CompleteUnlocking();
            }
            else
            {
                Debug.Log("Not enough gems to instantly unlock chest!");
            }
        }

        private void CompleteUnlocking()
        {
            chestController.Model.CompleteUnlocking();
            stateMachine.ChangeState(ChestState.UNLOCKED);
        }

        public void OnUnlockTimerComplete() => stateMachine.ChangeState(ChestState.UNLOCKED);
    }
}