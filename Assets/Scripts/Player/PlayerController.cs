namespace ChestSystem.Player
{
    public class PlayerController
    {
        private readonly PlayerModel playerModel;
        private readonly PlayerView playerView;

        public int CoinCount => playerModel.CoinCount;
        public int GemsCount => playerModel.GemsCount;

        public PlayerController(PlayerView _playerView)
        {
            playerView = _playerView;
            playerModel = new PlayerModel();

            playerView.SetPlayerController(this);
        }

        public void UpdateCoinCount(int newCoinValue)
        {
            playerModel.SetCoinCount(newCoinValue);
            playerView.UpdateCoinText();
        }

        public void UpdateGemsCount(int newGemsValue)
        {
            playerModel.SetGemsCount(newGemsValue);
            playerView.UpdateGemsText();
        }
    }
}