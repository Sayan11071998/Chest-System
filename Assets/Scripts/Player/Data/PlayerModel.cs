namespace ChestSystem.Player.Data
{
    public class PlayerModel
    {
        private int coinCount;
        private int gemsCount;

        public int CoinCount => coinCount;
        public int GemsCount => gemsCount;

        public void Initialize(PlayerScriptableObject playerData)
        {
            coinCount = playerData.coinCount;
            gemsCount = playerData.gemsCount;
        }

        public void UpdateCoinCount(int newCoinCount) => coinCount = newCoinCount;
        public void UpdateGemsCount(int newGemsCount) => gemsCount = newGemsCount;
    }
}