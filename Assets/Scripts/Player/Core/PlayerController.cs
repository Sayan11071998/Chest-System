using ChestSystem.Player.Data;
using ChestSystem.Player.UI;

namespace ChestSystem.Player.Core
{
    public class PlayerController
    {
        private PlayerView playerView;
        private PlayerModel playerModel;

        public PlayerView PlayerView => playerView;
        public PlayerModel PlayerModel => playerModel;

        public int CoinCount => playerModel.CoinCount;
        public int GemsCount => playerModel.GemsCount;

        public PlayerController(PlayerView view, PlayerModel model)
        {
            playerView = view;
            playerModel = model;
        }

        public void UpdateCoinCount(int newCoinCount)
        {
            playerModel.UpdateCoinCount(newCoinCount);
            playerView.UpdateCoinText();
        }

        public void UpdateGemsCount(int newGemsValue)
        {
            playerModel.UpdateGemsCount(newGemsValue);
            playerView.UpdateGemsText();
        }
    }
}