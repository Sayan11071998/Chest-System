using ChestSystem.Chest.Core;
using ChestSystem.Chest.Data;
using ChestSystem.Command.AbstractCommand;
using ChestSystem.Player.Core;

namespace ChestSystem.Command.ConcreteCommand
{
    public class InstantChestUnlockCommand : ICommand
    {
        private PlayerService playerService;
        private ChestController chestController;
        private int gemCost;
        private int previousPlayerGems;
        private float previousUnlockTime;
        private ChestState previousState;

        public void Execute(PlayerService playerService, ChestController chestController)
        {
            this.playerService = playerService;
            this.chestController = chestController;

            previousPlayerGems = playerService.PlayerController.GemsCount;
            previousUnlockTime = chestController.ChestModel.RemainingUnlockTime;
            previousState = chestController.CurrentState;

            gemCost = chestController.ChestModel.CurrentGemCost;

            playerService.PlayerController.UpdateGemsCount(previousPlayerGems - gemCost);

            chestController.ChestModel.CompleteUnlocking();
            chestController.ChestStateMachine.ChangeState(ChestState.UNLOCKED);
        }

        public void Undo()
        {
            if (playerService == null || chestController == null)
                return;

            playerService.PlayerController.UpdateGemsCount(previousPlayerGems);

            if (previousState == ChestState.UNLOCKING)
            {
                ChestModel model = chestController.ChestModel;

                var field = typeof(ChestModel).GetField("remainingUnlockTime",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                if (field != null)
                    field.SetValue(model, previousUnlockTime);

                model.UpdateGemCost();
                chestController.ChestStateMachine.ChangeState(ChestState.UNLOCKING);
            }
        }
    }
}