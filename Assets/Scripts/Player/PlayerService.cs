namespace ChestSystem.Player
{
    public class PlayerService
    {
        public PlayerController PlayerController { get; private set; }

        public PlayerService(PlayerView playerView) => PlayerController = new PlayerController(playerView);
    }
}