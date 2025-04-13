using ChestSystem.Chest.Data;
using ChestSystem.Chest.UI;
using TMPro;

namespace ChestSystem.Chest.Managers
{
    public class ChestStateManager
    {
        private ChestState currentState = ChestState.LOCKED;
        private TextMeshProUGUI statusText;
        private ChestView chestView;

        public ChestState CurrentState => currentState;

        public ChestStateManager(ChestView chestView)
        {
            this.chestView = chestView;
        }

        public void Initialize(TextMeshProUGUI statusText)
        {
            this.statusText = statusText;
            SetChestState(ChestState.LOCKED);
        }

        public void SetChestState(ChestState newState)
        {
            currentState = newState;

            switch (currentState)
            {
                case ChestState.LOCKED:
                    statusText.text = "LOCKED";
                    break;
                case ChestState.UNLOCKING:
                    statusText.text = "UNLOCKING...";
                    break;
                case ChestState.UNLOCKED:
                    statusText.text = "UNLOCKED";
                    break;
                case ChestState.COLLECTED:
                    statusText.text = "COLLECTED";
                    break;
            }
        }
    }
}