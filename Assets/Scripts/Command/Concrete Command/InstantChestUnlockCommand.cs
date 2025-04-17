using ChestSystem.Chest.Core;
using ChestSystem.Command.AbstractCommand;
using ChestSystem.Player.Core;

namespace ChestSystem.Command.ConcreteCommand
{
    public class InstantChestUnlockCommand : ICommand
    {
        public void Execute(PlayerService playerService, ChestController chestController)
        { }

        public void Undo()
        { }
    }
}