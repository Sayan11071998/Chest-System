namespace ChestSystem.Player
{
    public class PlayerService
    {
        public PlayerController PlayerController { get; private set; }

        public PlayerService(PlayerView playerView, PlayerScriptableObject playerScriptableObject) => PlayerController = new PlayerController(playerView, playerScriptableObject);
    }
}