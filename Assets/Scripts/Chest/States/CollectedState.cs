using ChestSystem.Chest.UI;
using ChestSystem.Chest.Core;
using ChestSystem.Utilities;
using ChestSystem.Core;
using UnityEngine;

namespace ChestSystem.Chest.States
{
    public class CollectedState : IState
    {
        protected ChestView chestView;
        protected ChestStateMachine stateMachine;

        public CollectedState(ChestView chestView, ChestStateMachine stateMachine)
        {
            this.chestView = chestView;
            this.stateMachine = stateMachine;
        }

        public virtual void OnStateEnter()
        {
            chestView.SetStatusText("COLLECTED");
            chestView.HideGemCost();

            // Collect the rewards
            CollectRewards();
        }

        private void CollectRewards()
        {
            // Get the controller from state machine
            ChestController controller = stateMachine.GetController();
            if (controller == null) return;
            
            int coinsAwarded, gemsAwarded;
            controller.CollectChest(chestView, out coinsAwarded, out gemsAwarded);
            
            // Update player resources
            var playerController = GameService.Instance.playerService.PlayerController;
            playerController.UpdateCoinCount(playerController.CoinCount + coinsAwarded);
            playerController.UpdateGemsCount(playerController.GemsCount + gemsAwarded);
            
            Debug.Log($"Collected chest: {chestView.ChestType}. Rewards: {coinsAwarded} coins, {gemsAwarded} gems");
        }

        public virtual void Update()
        {
            // No update needed for collected state
        }

        public virtual void OnStateExit()
        {
            // No exit actions needed
        }
    }
}