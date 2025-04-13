using ChestSystem.Chest;

namespace ChestSystem.Events
{
    public class ChestCollectedEventArgs
    {
        public ChestView Chest { get; private set; }
        public int CoinsAwarded { get; private set; }
        public int GemsAwarded { get; private set; }

        public ChestCollectedEventArgs(ChestView chest, int coinsAwarded, int gemsAwarded)
        {
            Chest = chest;
            CoinsAwarded = coinsAwarded;
            GemsAwarded = gemsAwarded;
        }
    }
}