namespace ChestSystem.Chest
{
    public class ChestSlot
    {
        public ChestModel currentChestModel { get; private set; }
        public ChestView currentChestView { get; private set; }

        public bool IsOccupied => currentChestModel != null;

        public void AssignChest(ChestModel _chestModel, ChestView _chestView)
        {
            currentChestModel = _chestModel;
            currentChestView = _chestView;
        }

        public void ClearSlot()
        {
            currentChestModel = null;
            currentChestView = null;
        }
    }
}