using ChestSystem.Chest.Core;
using ChestSystem.Chest.Data;
using ChestSystem.Command.AbstractCommand;
using ChestSystem.Player.Core;
using UnityEngine;

namespace ChestSystem.Command.ConcreteCommand
{
    public class InstantChestUnlockCommand : ICommand
    {
        private PlayerService playerService;
        private ChestController chestController;
        private int gemCost;
        private int previousPlayerGems;
        private float previousUnlockTime;
        private Sprite previousChestSprite;
        private ChestState previousState;

        public void Execute(PlayerService playerService, ChestController chestController)
        {
            this.playerService = playerService;
            this.chestController = chestController;

            previousPlayerGems = playerService.PlayerController.GemsCount;
            previousUnlockTime = chestController.ChestModel.RemainingUnlockTime;
            previousChestSprite = chestController.ChestModel.ChestSprite;
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

                var timeField = typeof(ChestModel).GetField("remainingUnlockTime",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                if (timeField != null)
                    timeField.SetValue(model, previousUnlockTime);

                var spriteField = typeof(ChestModel).GetField("chestSprite",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                if (spriteField != null)
                    spriteField.SetValue(model, previousChestSprite);

                model.UpdateGemCost();
                chestController.ChestStateMachine.ChangeState(ChestState.UNLOCKING);
                chestController.ChestView.UpdateChestSprite(previousChestSprite);
            }
        }
    }
}