namespace ChestSystem.Player
{
    public class PlayerController
    {
        private readonly PlayerScriptableObject playerScriptableObject;
        private readonly PlayerView playerView;

        public int CoinCount => playerScriptableObject.coinCount;
        public int GemsCount => playerScriptableObject.gemsCount;

        public PlayerController(PlayerView _playerView, PlayerScriptableObject _playerScriptableObject)
        {
            playerView = _playerView;
            playerScriptableObject = _playerScriptableObject;

            playerView.SetPlayerController(this);
        }

        public void UpdateCoinCount(int newCoinCount)
        {
            playerScriptableObject.coinCount = newCoinCount;
            playerView.UpdateCoinText();
        }

        public void UpdateGemsCount(int newGemsValue)
        {
            playerScriptableObject.gemsCount = newGemsValue;
            playerView.UpdateGemsText();
        }
    }
}