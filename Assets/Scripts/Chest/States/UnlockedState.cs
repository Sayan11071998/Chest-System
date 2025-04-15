using ChestSystem.Chest.Core;
using ChestSystem.Core;
using ChestSystem.Events;
using ChestSystem.Utilities;
using UnityEngine;

namespace ChestSystem.Chest
{
    public class UnlockedState : IState
    {
        private ChestStateMachine stateMachine;
        private ChestController chestController;

        public UnlockedState(ChestController chestController, ChestStateMachine stateMachine)
        {
            this.chestController = chestController;
            this.stateMachine = stateMachine;
        }

        public void OnStateEnter()
        {
            chestController.View.UpdateStatusText("UNLOCKED");
            chestController.View.UpdateTimerDisplay();
            chestController.View.SetGemCostVisible(false);
            chestController.OnUnlockCompleted();
        }

        public void OnStateExit() { }

        public void Update() { }

        public void HandleChestClicked()
        {
            CollectChest();
        }

        private void CollectChest()
        {
            int coinsAwarded, gemsAwarded;
            chestController.Model.GetChestData().CalculateRewards(out coinsAwarded, out gemsAwarded);

            var playerController = GameService.Instance.playerService.PlayerController;
            playerController.UpdateCoinCount(playerController.CoinCount + coinsAwarded);
            playerController.UpdateGemsCount(playerController.GemsCount + gemsAwarded);

            EventService.Instance.OnChestCollected.InvokeEvent(chestController.View, coinsAwarded, gemsAwarded);

            GameService.Instance.chestService.ReplaceChestWithEmptySlot(chestController.View);

            Debug.Log($"Collected chest: {chestController.View.ChestType}. Rewards: {coinsAwarded} coins, {gemsAwarded} gems");

            stateMachine.ChangeState(ChestState.COLLECTED);
        }
    }
}