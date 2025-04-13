using UnityEngine;
using ChestSystem.Core;

namespace ChestSystem.Chest
{
    public class ChestRewardManager
    {
        private ChestScriptableObject chestData;
        private ChestView chestView;

        public ChestRewardManager(ChestView chestView)
        {
            this.chestView = chestView;
        }

        public void Initialize(ChestScriptableObject chestData) => this.chestData = chestData;

        public void CollectChest(ChestView chest)
        {
            int coinsAwarded, gemsAwarded;
            GameService.Instance.chestService.CollectChest(chest, out coinsAwarded, out gemsAwarded);

            var playerController = GameService.Instance.playerService.PlayerController;
            playerController.UpdateCoinCount(playerController.CoinCount + coinsAwarded);
            playerController.UpdateGemsCount(playerController.GemsCount + gemsAwarded);

            Debug.Log($"Collected chest: {chest.ChestType}. Rewards: {coinsAwarded} coins, {gemsAwarded} gems");
        }
    }
}