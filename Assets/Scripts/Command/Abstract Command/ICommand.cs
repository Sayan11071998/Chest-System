using ChestSystem.Chest.Core;
using ChestSystem.Player.Core;

namespace ChestSystem.Command.AbstractCommand
{
    public interface ICommand
    {
        public void Execute(PlayerService playerService, ChestController chestController);
        public void Undo();
    }
}