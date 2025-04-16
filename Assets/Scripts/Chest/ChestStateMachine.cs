using System.Collections.Generic;
using ChestSystem.Chest.Core;
using ChestSystem.Chest.Data;
using ChestSystem.Chest.States;
using ChestSystem.Utilities;

namespace ChestSystem.Chest
{
    public class ChestStateMachine
    {
        private IState currentState;
        public Dictionary<ChestState, IState> states;
        private ChestState currentChestStateEnum;

        public ChestStateMachine(ChestController chestController)
        {
            CreateStates(chestController);
        }

        private void CreateStates(ChestController chestController)
        {
            states = new Dictionary<ChestState, IState>()
            {
                {ChestState.LOCKED, new LockedState(chestController, this)},
                {ChestState.UNLOCKING, new UnlockingState(chestController, this)},
                {ChestState.UNLOCKED, new UnlockedState(chestController, this)},
                {ChestState.COLLECTED, new CollectedState(chestController, this)}
            };
        }

        public void ChangeState(ChestState newState)
        {
            if (states.ContainsKey(newState))
            {
                currentChestStateEnum = newState;
                ChangeState(states[newState]);
            }
        }

        private void ChangeState(IState newState)
        {
            currentState?.OnStateExit();
            currentState = newState;
            currentState?.OnStateEnter();
        }

        public void Update() => currentState?.Update();

        public IState GetCurrentState() => currentState;
        public ChestState GetCurrentChestState() => currentChestStateEnum;
        public Dictionary<ChestState, IState> GetStates() => states;
    }
}