using ChestSystem.Chest.UI;
using ChestSystem.Chest.Core;
using ChestSystem.Utilities;
using ChestSystem.Core;
using UnityEngine;

namespace ChestSystem.Chest.States
{
    public class UnlockingState : IState
    {
        protected ChestView chestView;
        protected ChestStateMachine stateMachine;

        public UnlockingState(ChestView chestView, ChestStateMachine stateMachine)
        {
            this.chestView = chestView;
            this.stateMachine = stateMachine;
        }

        public virtual void OnStateEnter()
        {
            chestView.SetStatusText("UNLOCKING");
            chestView.ShowGemCost();

            // Start the unlocking process
            chestView.StartUnlockingProcess();

            // Register with controller as currently unlocking chest
            ChestController controller = stateMachine.GetController();
            if (controller != null)
            {
                controller.SetUnlockingChest(chestView);
            }
        }

        public virtual void Update()
        {
            // State updates are handled by the coroutine in ChestView
            // The model is updated there and this state just responds to changes
        }

        public virtual void OnStateExit()
        {
            // Notify controller that unlocking is complete when exiting this state
            ChestController controller = stateMachine.GetController();
            if (controller != null)
            {
                controller.ChestUnlockCompleted(chestView);
            }
        }

        public virtual void AttemptInstantUnlock()
        {
            // Check if player has enough gems
            int playerGems = GameService.Instance.playerService.PlayerController.GemsCount;
            int requiredGems = chestView.GetCurrentGemCost();

            if (playerGems >= requiredGems)
            {
                // Deduct gems
                GameService.Instance.playerService.PlayerController.UpdateGemsCount(playerGems - requiredGems);

                // Complete unlock immediately
                chestView.CompleteUnlocking();

                // Change state to unlocked
                stateMachine.ChangeState(Data.ChestState.UNLOCKED);
            }
            else
            {
                Debug.Log("Not enough gems to instantly unlock chest!");
            }
        }
    }
}