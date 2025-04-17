using ChestSystem.Player.Data;
using ChestSystem.Player.UI;

namespace ChestSystem.Player.Core
{
    public class PlayerService
    {
        private PlayerController playerController;
        private PlayerModel playerModel;

        public PlayerController PlayerController => playerController;

        public PlayerService(PlayerView playerView, PlayerScriptableObject playerData)
        {
            playerModel = new PlayerModel();
            playerModel.Initialize(playerData);

            playerController = new PlayerController(playerView, playerModel);

            playerView.SetController(playerController);
        }
    }
}