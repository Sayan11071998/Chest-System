using ChestSystem.Command.AbstractCommand;
using ChestSystem.UI.Core;
using System.Collections.Generic;
using ChestSystem.Events;
using ChestSystem.UI.Data;

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
                string title = UIStrings.InstantUnlockCompleted;
                string message = UIStrings.ChestInstantlyUnlockedWithGems;
                string buttonText = UIStrings.Okay;
                string undoButtonText = UIStrings.Undo;

                NotificationManager.Instance.ShowNotificationWithUndo(title, message, buttonText, undoButtonText);
            }
        }
    }
}