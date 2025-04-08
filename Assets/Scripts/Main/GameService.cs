using ChestSystem.Player;
using ChestSystem.Utilities;
using UnityEngine;

namespace ChestSystem.Main
{
    public class GameService : GenericMonoSingelton<GameService>
    {
        public PlayerService playerService { get; private set; }

        [Header("Player")]
        [SerializeField] private PlayerView playerView;

        protected override void Awake()
        {
            base.Awake();

            playerService = new PlayerService(playerView);
        }
    }
}