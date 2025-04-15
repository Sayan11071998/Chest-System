using ChestSystem.Chest.UI;
using ChestSystem.Core;
using ChestSystem.Events;
using UnityEngine;

namespace ChestSystem.Chest.Core
{
    public class ChestController
    {
        private ChestView view;
        private ChestModel model;
        private bool isRegisteredAsUnlocking = false;

        public ChestController(ChestView view, ChestModel model)
        {
            this.view = view;
            this.model = model;
        }

        public void HandleChestClicked()
        {
            switch (model.CurrentState)
            {
                case ChestState.LOCKED:
                    AttemptStartUnlocking();
                    break;

                case ChestState.UNLOCKING:
                    AttemptInstantUnlock();
                    break;

                case ChestState.UNLOCKED:
                    CollectChest();
                    break;
            }
        }

        private void AttemptStartUnlocking()
        {
            var chestService = GameService.Instance.chestService;

            if (chestService.CanStartUnlocking())
            {
                chestService.SetUnlockingChest(view);
                model.StartUnlocking(view, this);
                view.SetGemCostVisible(true);
                view.UpdateStatusText();
                isRegisteredAsUnlocking = true;
            }
            else
            {
                Debug.Log("Another chest is already being unlocked!");
            }
        }

        private void AttemptInstantUnlock()
        {
            int playerGems = GameService.Instance.playerService.PlayerController.GemsCount;

            if (playerGems >= model.CurrentGemCost)
            {
                GameService.Instance.playerService.PlayerController.UpdateGemsCount(playerGems - model.CurrentGemCost);
                CompleteUnlocking();
            }
            else
            {
                Debug.Log("Not enough gems to instantly unlock chest!");
            }
        }

        private void CompleteUnlocking()
        {
            model.CompleteUnlocking(view, this);
            view.SetGemCostVisible(false);
            view.UpdateStatusText();
            view.UpdateTimerDisplay();
        }

        public void OnUnlockCompleted()
        {
            GameService.Instance.chestService.OnChestUnlockCompleted(view);
            view.SetGemCostVisible(false);
            isRegisteredAsUnlocking = false;
        }

        private void CollectChest()
        {
            int coinsAwarded, gemsAwarded;
            model.GetChestData().CalculateRewards(out coinsAwarded, out gemsAwarded);

            var playerController = GameService.Instance.playerService.PlayerController;
            playerController.UpdateCoinCount(playerController.CoinCount + coinsAwarded);
            playerController.UpdateGemsCount(playerController.GemsCount + gemsAwarded);

            view.SetState(ChestState.COLLECTED);
            EventService.Instance.OnChestCollected.InvokeEvent(view, coinsAwarded, gemsAwarded);

            GameService.Instance.chestService.ReplaceChestWithEmptySlot(view);

            Debug.Log($"Collected chest: {view.ChestType}. Rewards: {coinsAwarded} coins, {gemsAwarded} gems");
        }

        public void Cleanup()
        {
            if (isRegisteredAsUnlocking && model.IsUnlocking)
            {
                model.StopUnlocking(view);
                GameService.Instance.chestService.OnChestUnlockCompleted(view);
                isRegisteredAsUnlocking = false;
            }
        }
    }
}