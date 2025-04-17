using ChestSystem.CommandAction.Actions;

namespace ChestSystem.CommandAction
{
    public class ActionService
    {
        private InstantChestUnlockAction instantChestUnlockAction;

        public ActionService()
        {
            CreateActions();
        }

        private void CreateActions() => instantChestUnlockAction = new InstantChestUnlockAction();

        public InstantChestUnlockAction InstantChestUnlockAction => instantChestUnlockAction;
    }
}