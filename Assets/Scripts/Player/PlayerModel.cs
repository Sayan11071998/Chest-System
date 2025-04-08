namespace ChestSystem.Player
{
    public class PlayerModel
    {
        private int initialCoinCount = 50;
        private int initialGemsCount = 50;

        public int CoinCount { get; private set; }
        public int GemsCount { get; private set; }

        public PlayerModel()
        {
            CoinCount = initialCoinCount;
            GemsCount = initialGemsCount;
        }

        public void SetCoinCount(int value) => CoinCount = value;
        public void SetGemsCount(int value) => GemsCount = value;
    }
}