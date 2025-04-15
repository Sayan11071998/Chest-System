using ChestSystem.Chest.UI;
using ChestSystem.Chest.Core;
using ChestSystem.Utilities;

namespace ChestSystem.Chest.States
{
    public class UnlockedState : IState
    {
        protected ChestView chestView;
        protected ChestStateMachine stateMachine;

        public UnlockedState(ChestView chestView, ChestStateMachine stateMachine)
        {
            this.chestView = chestView;
            this.stateMachine = stateMachine;
        }

        public virtual void OnStateEnter()
        {
            chestView.SetStatusText("UNLOCKED");
            chestView.SetTimerText("Ready!");
            chestView.HideGemCost();
        }

        public virtual void Update()
        {
            // No update needed for unlocked state
        }

        public virtual void OnStateExit()
        {
            // No exit actions needed
        }
    }
}