using ChestSystem.Command.AbstractCommand;
using ChestSystem.UI.Core;
using System.Collections.Generic;
using ChestSystem.Events;

namespace ChestSystem.Command
{
    public class CommandInvoker
    {
        private Stack<ICommand> commandHistory;

        public CommandInvoker()
        {
            commandHistory = new Stack<ICommand>();
        }

        public void ExecuteCommand(ICommand command)
        {
            if (command == null)
                return;

            commandHistory.Push(command);

            ShowUndoOption();
        }

        public void Undo()
        {
            if (commandHistory.Count > 0)
            {
                ICommand lastCommand = commandHistory.Pop();
                lastCommand.Undo();
                EventService.Instance.OnCommandUndo.InvokeEvent();
            }
        }

        private void ShowUndoOption()
        {
            if (commandHistory.Count > 0)
            {
                string title = "INSTANT UNLOCK COMPLETED";
                string message = "Chest has been instantly unlocked with gems.";
                string buttonText = "OK";
                string undoButtonText = "UNDO";

                NotificationManager.Instance.ShowNotificationWithUndo(title, message, buttonText, undoButtonText);
            }
        }
    }
}