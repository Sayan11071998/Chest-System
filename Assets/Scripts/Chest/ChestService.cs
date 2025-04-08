using System.Collections.Generic;

namespace ChestSystem.Chest
{
    public class ChestService
    {
        public ChestController chestController;

        public ChestService(List<ChestScriptableObject> chests) => chestController = new ChestController(chests);

        public void GenerateRandomChest() => chestController.GenerateRandomChest();
    }
}