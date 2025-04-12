namespace ChestSystem.Chest
{
    public class ChestModel
    {
        public ChestScriptableObject chestScriptableObject;
        public ChestState chestState;

        public ChestModel(ChestScriptableObject _chestScriptableObject) => chestScriptableObject = _chestScriptableObject;
    }
}