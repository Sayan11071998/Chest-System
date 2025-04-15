using ChestSystem.Chest.Data;
using ChestSystem.Chest.States;
using ChestSystem.Chest.UI;
using ChestSystem.Chest.Core;
using System.Collections.Generic;
using ChestSystem.Utilities;

namespace ChestSystem.Chest.Core
{
    public class ChestStateMachine
    {
        private Dictionary<ChestState, IState> states;
        private IState currentState;
        private ChestView chestView;
        private ChestController controller;

        public void Initialize(ChestView chestView, ChestController controller)
        {
            this.chestView = chestView;
            this.controller = controller;

            // Create all possible states
            states = new Dictionary<ChestState, IState>
            {
                { ChestState.LOCKED, new LockedState(chestView, this) },
                { ChestState.UNLOCKING, new UnlockingState(chestView, this) },
                { ChestState.UNLOCKED, new UnlockedState(chestView, this) },
                { ChestState.COLLECTED, new CollectedState(chestView, this) }
            };
        }

        public void ChangeState(ChestState newState)
        {
            if (currentState != null)
                currentState.OnStateExit();

            if (states.TryGetValue(newState, out IState nextState))
            {
                currentState = nextState;
                chestView.GetModel().SetState(newState);
                currentState.OnStateEnter();
            }
            else
            {
                UnityEngine.Debug.LogError($"State {newState} not found in state machine");
            }
        }

        public void HandleClick()
        {
            if (currentState is LockedState)
            {
                if (controller.CanStartUnlocking())
                    ChangeState(ChestState.UNLOCKING);
                else
                    UnityEngine.Debug.Log("Cannot start unlocking - another chest is already being unlocked");
            }
            else if (currentState is UnlockingState)
            {
                // Jump to unlocked if player has enough gems
                if (chestView is UnlockingState unlockingState)
                {
                    unlockingState.AttemptInstantUnlock();
                }
            }
            else if (currentState is UnlockedState)
            {
                // Collect the chest
                ChangeState(ChestState.COLLECTED);
            }
        }

        public void Update()
        {
            currentState?.Update();
        }

        public ChestState GetCurrentStateType()
        {
            if (currentState is LockedState) return ChestState.LOCKED;
            if (currentState is UnlockingState) return ChestState.UNLOCKING;
            if (currentState is UnlockedState) return ChestState.UNLOCKED;
            if (currentState is CollectedState) return ChestState.COLLECTED;
            return ChestState.LOCKED; // Default
        }

        public IState GetState(ChestState stateType)
        {
            if (states.TryGetValue(stateType, out IState state))
                return state;
            return null;
        }
    }
}