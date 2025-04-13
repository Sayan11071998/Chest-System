using ChestSystem.Player.Data;
using ChestSystem.Player.UI;

namespace ChestSystem.Player.Core
{
    public class PlayerService
    {
        public PlayerController PlayerController { get; private set; }

        public PlayerService(PlayerView playerView, PlayerScriptableObject playerScriptableObject) => PlayerController = new PlayerController(playerView, playerScriptableObject);
    }
}