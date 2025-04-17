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

        public PlayerController PlayerController => playerController;

        public void SetController(PlayerController controller)
        {
            playerController = controller;
            UpdateUI();
        }

        private void UpdateUI()
        {
            UpdateCoinText();
            UpdateGemsText();
        }

        public void UpdateCoinText()
        {
            if (playerController != null)
                coinText.text = playerController.CoinCount.ToString();
        }

        public void UpdateGemsText()
        {
            if (playerController != null)
                gemsText.text = playerController.GemsCount.ToString();
        }
    }
}