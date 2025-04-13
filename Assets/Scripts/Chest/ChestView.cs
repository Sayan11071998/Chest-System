using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChestSystem.Chest
{
    public class ChestView : MonoBehaviour
    {
        [SerializeField] private Image chestImage;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private Button chestButton;

        private ChestScriptableObject chestData;
        private ChestType chestType;

        public void Initialize(ChestScriptableObject chestData)
        {
            this.chestData = chestData;
            this.chestType = chestData.chestType;

            InitializeChest(chestData);
        }

        private void InitializeChest(ChestScriptableObject chestData)
        {
            if (chestImage != null && chestData.chestSprite != null)
                chestImage.sprite = chestData.chestSprite;

            statusText.text = "LOCKED";
            timerText.text = CalculateUnlockTime();
            chestButton.onClick.AddListener(OnChestClicked);
        }

        private string CalculateUnlockTime()
        {
            float time = chestData.unlockTimeInSeconds;

            if (time < 3600)
            {
                int minutes = (int)(time / 60);
                int seconds = (int)(time % 60);
                return string.Format("{0} min {1} sec", minutes, seconds);
            }
            else
            {
                int hours = (int)(time / 3600);
                int minutes = (int)((time % 3600) / 60);
                return string.Format("{0} h {1} min", hours, minutes);
            }
        }

        private void OnChestClicked()
        {
            Debug.Log($"Chest clicked: {chestType}");
        }

        public void OnReturnToPool()
        {
            chestButton.onClick.RemoveAllListeners();
        }
    }
}