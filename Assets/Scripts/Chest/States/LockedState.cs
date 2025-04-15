using ChestSystem.Chest.Core;
using ChestSystem.Chest.UI;
using ChestSystem.Utilities;

namespace ChestSystem.Chest.States
{
    public class LockedState : IState
    {
        private ChestStateMachine stateMachine;
        protected ChestView chestView;
    
        public LockedState(ChestView chestView, ChestStateMachine stateMachine)
        {
            this.chestView = chestView;
            this.stateMachine = stateMachine;
        }

        public virtual void OnStateEnter()
        {
            chestView.SetStatusText("LOCKED");
            chestView.SetTimerText(chestView.GetModel().GetFormattedTimeRemaining());
            chestView.HideGemCost();
        }

        public virtual void Update()
        {
            // No update needed for locked state
        }

        public virtual void OnStateExit()
        {
            // No exit actions needed
        }
    }
}