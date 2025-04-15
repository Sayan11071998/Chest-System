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
            chestView.CollectRewards();
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