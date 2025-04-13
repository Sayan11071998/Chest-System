using ChestSystem.Player.Core;
using TMPro;
using UnityEngine;

namespace ChestSystem.Player.UI
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coinText;
        [SerializeField] private TextMeshProUGUI gemsText;

        private PlayerController playerController;

        public void SetPlayerController(PlayerController _playerController)
        {
            playerController = _playerController;
            UpdateUI();
        }

        private void UpdateUI()
        {
            UpdateCoinText();
            UpdateGemsText();
        }

        public void UpdateCoinText() => coinText.text = playerController.CoinCount.ToString();
        public void UpdateGemsText() => gemsText.text = playerController.GemsCount.ToString();
    }
}